import AVFoundation
import CoreLocation
import UIKit

public class Permissions : NSObject
{
    @objc public static func CheckCameraPermission() -> Int
    {
        let cameraAuthorizationStatus = AVCaptureDevice.authorizationStatus(for: .video)
        
        switch(cameraAuthorizationStatus) {
            case .notDetermined:
                return 0
            case .authorized:
                return 1
            case .denied:
                return -1
            default:
                return -2
        }
    }
    
    @objc public static func RequestCameraPermission()
    {
        AVCaptureDevice.requestAccess(for: .video) { (result) in }
    }

    @objc public static func CheckLocationPermission() -> Int
    {
        if CLLocationManager.locationServicesEnabled()
        {
            switch CLLocationManager.authorizationStatus() {
            case .notDetermined:
                return 0
            case .authorizedAlways, .authorizedWhenInUse:
                if #available(iOS 14.0, *)
                {
                    let locationManager = CLLocationManager()
                    if locationManager.accuracyAuthorization == .reducedAccuracy {
                        return 0
                    }
                }
                return 1
            case .denied:
                return -1
            default:
                return -2;
            }
        }
        return -2;
    }
    
    @objc public static func RequestGPSPermission()
    {
        let locationManager = CLLocationManager()
        switch CLLocationManager.authorizationStatus() {
        case .notDetermined :
            locationManager.requestWhenInUseAuthorization()
        case .authorizedAlways, .authorizedWhenInUse:
            if #available(iOS 14.0, *) {
                if locationManager.accuracyAuthorization == .reducedAccuracy {
                    locationManager.requestTemporaryFullAccuracyAuthorization(withPurposeKey: "TemporaryAuth", completion: { (err) in
                        locationManager.accuracyAuthorization
                    })
                }
            }
        default:
            break
        }
    }
    
    @objc public static func OpenAppSettings()
    {
        UIApplication.shared.open(URL(string: UIApplication.openSettingsURLString)!);
    }
}
