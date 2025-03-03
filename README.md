# AutoFill

## Description
The AutoFill project is an application that uses artificial intelligence to detect and automatically fill fields in documents. It is composed of several services and repositories that work together to provide this functionality.

## Project Structure
- **RuiSantos.AutoFill.Application**: Contains application logic and services.
- **RuiSantos.AutoFill.Domain**: Defines domain entities and interfaces.
- **RuiSantos.AutoFill.Infrastructure**: Implements repositories and infrastructure services.
- **RuiSantos.AutoFill.Tests**: Contains unit and integration tests.
 
## Technologies Used
- **C\#**
- **.NET Core**
- **XUnit**: Testing framework.
- **NSubstitute**: Library for creating mocks.
- **Microsoft.Extensions.Logging**: Logging library.
- **LiteDB**: Embedded NoSQL database.

## Configuration and Execution
### Prerequisites
- .NET Core SDK 6.0 or higher

### Installation
1. Clone the repository:
    ```sh
    git clone https://github.com/ruisantos78/AutoFill.git
    cd AutoFill
    ```

2. Restore dependencies:
    ```sh
    dotnet restore
    ```

### Execution
To run the application:

```sh
   dotnet run --project RuiSantos.AutoFill.Application
```

### Tests
To run the tests

```sh
   dotnet test
```

### Contribution
1. Fork the project.
2. Create a branch for your feature (`git checkout -b feature/new-feature`).
3. Commit your changes (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature/new-feature`).
5. Open a Pull Request.
