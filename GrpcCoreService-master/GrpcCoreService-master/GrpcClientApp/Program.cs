using Grpc.Net.Client;
using GrpcClientApp;

var channel = GrpcChannel.ForAddress("http://localhost:5181");
var client = new Employee.EmployeeClient(channel);

Console.WriteLine("Enter Employee ID....");
var inputStr = Console.ReadLine();

int empId = 0;
if (!int.TryParse(inputStr, out empId))
{
    Console.WriteLine("This is invalid Employee ID, please exit and try again....");
}
else
{
    var emp = new GetEmpDetail { EmpId = empId };

    var serverResponse = await client.GetEmpInformationAsync(emp);
    Console.WriteLine("Employee Name | Role | Email ID | Department");
    Console.WriteLine($"{serverResponse.EmpName} | {serverResponse.EmpRole} | {serverResponse.EmpEmail} | {serverResponse.EmpDepartment}");
}
Console.WriteLine("Press any key to exit...");
Console.ReadKey();


