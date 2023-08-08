using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class PostProcessSettings
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget != BuildTarget.iOS) return;

        const string plistLocationKey = "NSLocationTemporaryUsageDescriptionDictionary";
        const string temporaryKey = "TemporaryAuth";

        string plistPath = Path.Combine(path, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);
        PlistElementDict root = plist.root;

        PlistElementDict usageDescDict = new PlistElementDict();
        PlistElementString value = new PlistElementString("");
        usageDescDict[temporaryKey] = value;

        root[plistLocationKey] = usageDescDict;
        root.SetBoolean("UIFileSharingEnabled", true);

        plist.WriteToFile(plistPath);

        var projectPath = PBXProject.GetPBXProjectPath(path);

        var project = new PBXProject();
        project.ReadFromFile(projectPath);

        var targetGuid = project.GetUnityMainTargetGuid();

        project.SetBuildProperty(targetGuid, "SUPPORTS_MAC_DESIGNED_FOR_IPHONE_IPAD", "NO");

        try
        {
            var projectInString = File.ReadAllText(projectPath);
            projectInString = projectInString.Replace("SUPPORTS_MAC_DESIGNED_FOR_IPHONE_IPAD = YES;",
                $"SUPPORTS_MAC_DESIGNED_FOR_IPHONE_IPAD = NO;");
            File.WriteAllText(projectPath, projectInString);
        }
        catch
        {

        }

        project.WriteToFile(projectPath);
    }
}