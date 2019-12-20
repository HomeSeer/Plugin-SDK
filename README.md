[![Release Version][release-badge]][nuget-package-url]
[![NuGet Downloads][nuget-downloads]][nuget-package-url]

[![Last Commit][maintained-badge]][commits-url]
[![Open Issues][open-issues]][issues-url]
[![Open Pull Requests][open-pull-requests]][pull-requests-url]

[![HomeSeer Version][hs-version-badge]][hs-version-url]

# HomeSeer Plugin SDK
A software development kit used to create plugins for the [HomeSeer][homeseer-url] platform.

# Using the SDK
This SDK can be downloaded and built from source, but, for the purpose of plugin development, we recommend you add the [NuGet package][nuget-package-url] to your project to make sure you are using the latest, stable version. that maps to the HS4 version listed above.  The code associated with this version is available in the [master branch][master-branch].

Upcoming changes and bug fixes can be found in the [dev branch][dev-branch], but please keep in mind that there is no guarantee that the code in this branch will work with the version of HS4 you have running due to how the two packages are integrated.

# Documentation
You can find a full API reference and a getting started guide in the [Plugin SDK documentation][plugin-sdk-docs].

# Questions and Contributions
Please submit any questions, bugs, and requests as [issues][issues-url] in this repository.  Contributions to this SDK are encouraged and welcome.  Fork this repository and submit a [pull request][pull-requests-url].

# License
This SDK is available for your use according to the [GNU Affero General Public License][license-url]. Please also make sure to read and familiarize yourself with HomeSeer's [3rd-party development distribution policy][distribution-policy] if you are looking to publish any work using this SDK.

<!-- Images & Badges -->

[release-badge]: https://img.shields.io/nuget/v/HomeSeer-PluginSDK
[hs-version-badge]: https://img.shields.io/badge/Works%20With-HS4.0.1.2-blue
[maintained-badge]: https://img.shields.io/github/last-commit/HomeSeer/Plugin-SDK
[hs-logo]: http://homeseer.com/images/HS4/hs4-64.png
[nuget-downloads]: https://img.shields.io/nuget/dt/HomeSeer-PluginSDK
[open-issues]: https://img.shields.io/github/issues-raw/HomeSeer/Plugin-SDK
[open-pull-requests]: https://img.shields.io/github/issues-pr-raw/HomeSeer/Plugin-SDK

<!-- URLs -->
[hs-version-url]: https://homeseer.com/
[distribution-policy]: https://homeseer.com/3rd-party-development-distribution-policy/
[homeseer-url]: https://homeseer.com/
[plugin-sdk-docs]: https://docs.homeseer.com/display/HSPI
[nuget-package-url]: https://www.nuget.org/packages/HomeSeer-PluginSDK/
[issues-url]: https://github.com/HomeSeer/Plugin-SDK/issues
[pull-requests-url]: https://github.com/HomeSeer/Plugin-SDK/pulls
[commits-url]: https://github.com/HomeSeer/Plugin-SDK/commits/master
[license-url]: https://www.gnu.org/licenses/agpl-3.0.en.html
[master-branch]: https://github.com/HomeSeer/Plugin-SDK/tree/master
[dev-branch]: https://github.com/HomeSeer/Plugin-SDK/tree/dev
