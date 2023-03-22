# AR Navigation Lite SDK (Unity3D)

This is **AR Navigation Lite** SDK for Unity3D engine. The app use AR Foundation tracking system, VPS localization system and built-in unity route building system for navigation in indoor and outdoor locations. 

Main features are:
- Building routes from current position to chosen point in augmented reality
- Adding your locations and points of interest / editing points of interest in example location
- Built-in reactions on different events during the navigation session - localisation, reaching the point, hiding application, etc. with an opportunity of overriding and extensions
- Built-in building routes tool (only within one location)
- Built-in application UI/UX for navigation work with an opportunity of flexible settings - customize states order, adding custom states
- Adding 3D content in augmented reality

If you want access to other VPS locations or want to scan your own property, please contact us at <info@naviar.io>.

## Requirements

- Unity 2021.1+
- ARKit or ARCore supported device

## Installation

Just clone this repository. Requires to use --recurse-submodules

## Examples

AR Navigation Lite includes an example scene with an example AR navigation on the basic location. Load project in Unity Editor and open `Scenes/Main`. 

You can run this scene in Editor or build it on your mobile device.

## Usage

### Testing your app

Start application in Editor. At the beginning you will be shown a choose point state. You can choose any point from list and application will go to localisation state. After success localisation the route to chosen point will be build. You can press Tab for turn on mock moving move: it allows you moving around the location using WASD. When you will reach the final point a corresponding message will be shown. Pay attention, if you hide application на device during the route, app will suggest to restore route to chosen point.

You can flexibly customize states order, app logic and reaction to navigation events using AppRouter, AppModel and ARNavigationSession scripts. Learn more in the documentation

## License 

This project is licensed under [MIT License](LICENSE).
