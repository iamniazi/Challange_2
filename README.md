# Challenge 2: .NET Testing with NUnit and Allure

## Prerequisites
Ensure the following dependencies are installed on your system:
- **.NET SDK 6**
- **NUnit** (testing framework for .NET)
- **Allure.NUnit** (for generating test reports)
- **Microsoft.NET.Test.Sdk** (required for testing projects)
- **NUnit3TestAdapter** (adapter for running NUnit tests)

## Installation Instructions

1. **Restore and Build the Project:**
   ```bash
   dotnet restore
   dotnet build
   ```

2. **Ensure Required NuGet Packages are Installed:**
   - **NUnit**
   - **Allure.NUnit**
   - **Microsoft.NET.Test.Sdk**
   - **NUnit3TestAdapter**

## Running Tests

To execute the tests and generate Allure results:

1. **Run Tests:**
   ```bash
   dotnet test --test-adapter-path:.
   ```
   This will execute all tests in the project and save the results in the `bin\Debug\net6.0\allure-results` directory.

2. **Log File Location and Details**

The application generates log files to record execution details and activities. These logs are stored in the following location:

- **Directory:** `bin\Debug\net6.0\`
- **File Name:** `TestExecutionLogs`


3. **Generate and Serve Allure Report:**
   ```bash
   allure serve allure-results
   ```
   This will generate the Allure report and open it in your default web browser.

## Notes
- Ensure that the test results are stored in the `bin\Debug\net6.0\allure-results` directory.
- For additional configurations or advanced options, refer to the official documentation for [NUnit](https://nunit.org/) and [Allure](https://docs.qameta.io/allure/).
