#import <Foundation/Foundation.h>
#include "UnityFramework/UnityFramework-Swift.h"

extern "C" {
    void _ex_callCheckCameraPermission()
    {
        [Permissions CheckCameraPermission];
    }

    void _ex_callRequestCameraPermission()
    {
        [Permissions RequestCameraPermission];
    }

    void _ex_callCheckLocationPermission()
    {
        [Permissions CheckLocationPermission];
    }

    void _ex_callRequestLocationPermission()
    {
        [Permissions RequestGPSPermission];
    }

    void _ex_callOpenAppSettings()
    {
        [Permissions OpenAppSettings];
    }
}
