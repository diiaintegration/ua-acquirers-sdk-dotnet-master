Project Overview

The Diia API SDK allows developers to integrate with the Diia digital services platform for functionalities like document signing and sharing. This SDK enables:
- Authentication to securely access API.
- Document signing with multiple algorithm options.
- Branch and Offer Management.
- Obtaining deeplinks for document sharing and signing scenarios.

Project Contents

- DiiaClient.CryptoAPI - API of crypto-service used in SDK
- DiiaClient.CryptoService.UAPKI - crypto-service implementation based on library UAPKI
- DiiaClient.Example.Web - simple web-application to demonstrate SDK usage
- DiiaClient.Example - simple console-application to demonstrate SDK usage
- DiiaClient.DocUpload - simple server application to receive documents sent from Diia (and decrypt them (optional))
- DiiaClient.SDK - SDK library
- DiiaClient.SDK.Tests - Tests SDK library

Dependencies
- FluentAssertions (6.6.0): Provides readable syntax for asserting conditions in tests.
- Microsoft.NET.Test.Sdk (16.11.0): Required for running tests in .NET projects.
- WireMock.Net (1.4.40): Used for creating HTTP mocks for integration testing.
- xUnit (2.4.1): A popular test framework used for unit testing.
- xUnit.runner.visualstudio (2.4.3): Adds Visual Studio test runner support for xUnit tests.

The SDK requires certain dependencies to function correctly. When you install the SDK via NuGet, these dependencies are installed automatically. Below are the primary dependencies and their roles:


Diia API SDK for .NET Overview The Diia API SDK for .NET provides a simple and effective way to integrate with the Diia digital services platform, allowing developers to implement functionalities such as document signing and sharing. This SDK facilitates seamless interactions with the Diia API, enabling users to authenticate, create branches, manage offers, and generate links for signing documents.

Features Authentication: Easily obtain and manage session tokens for secure API access. Document Signing: Generate signing links for documents, utilizing various signing algorithms. Branch and Offer Management: Create, retrieve, and delete branches and offers associated with your application. Dynamic Linking: Create deep links for document signing and sharing, ensuring a smooth user experience. Installation To install the SDK, simply add it to your project using Composer:

Contributing Contributions are welcome! If you would like to contribute to the SDK, please fork the repository and submit a pull request. For any issues or feature requests, please open an issue in the GitHub repository.

License This SDK is licensed under the MIT License. See the LICENSE file for more information.

Testing

The SDK includes a series of test files in the `DiiaClient.SDK.Tests/` directory to validate the functionalities, such as `DiiaAPIAuthTest.php` for authentication and `DiiaAPICreateOfferTest.php` for offer creation. To run the tests, use:
```bash
vendor/bin/phpunit tests
```


Our scenarios: 
Sharing scenario
Review Technical Documentation
Please review the general technical documentation available here.
Diia Signature scenario
Review Technical Documentation
Please review the general technical documentation available here.

Obtaining a Test Token
To obtain a test token, please complete the application form here.
These steps provide initial access to the API for testing and preparing your integration with the system.

Important links
https://integration.diia.gov.ua/en/home.html - description of all available services
https://t.me/AiDiiaStartBot -reach us out to start the integration

