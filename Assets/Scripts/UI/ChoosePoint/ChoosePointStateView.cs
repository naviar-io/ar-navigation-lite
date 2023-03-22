using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace naviar.ARNavigation.UI
{
	public class ChoosePointStateView : MonoBehaviour
	{
        public PointButtonUI ObjectPrefab;
        public Transform PointsParent;

        public event System.Action<string> OnChoosePoint;

        public void CreatePointsUI(List<PlanPoint> points)
        {
            ClearCategories();
            foreach (var point in points)
            {
                PointButtonUI pointButton = Instantiate(ObjectPrefab, PointsParent);

                pointButton.SetName(point.PointID);
                pointButton.SetIcon(Sprite.Create(point.Icon, new Rect(0, 0, point.Icon.width, point.Icon.height), Vector2.zero));
                pointButton.AddListener(() => OnChoosePoint?.Invoke(point.PointID));
            }
        }

        public void ClearCategories()
        {
            foreach (Transform child in PointsParent)
                Destroy(child.gameObject);
        }
    }
}