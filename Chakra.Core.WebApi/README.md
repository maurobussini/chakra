Charka.Core.WebApi
===

A bunch of ASP.NET Core action filters and utilities for trace request, responses, durations and detailed exceptions, in the super-easy and super-pragmatic way.

### TraceOnFileError attribute

Given the following Controller, just decorate with **[TraceOnFileError]** attribute...

```csharp
[Route("api/Authentication")]
[TraceOnErrorFile]
public class AuthenticationController: Controller
{
	[HttpPost]
	[AllowAnonymous]
	[Route("SignIn")]
	[ProducesResponseType(typeof(SignInResult), 200)]
	public IActionResult SignIn([FromBody]SignInRequest request)
	{
		// Omitted for brevity ...	  
	}		
}
```

In case of exception, a file with details of occurred issue is generated in this location:

```
[application-root]
+- App_Data
|   +- traced-errors
|       +- 2020-01-23
|          +- Error_2020-01-23_15.34.28_209.log
```

and will contains something like this:

```
REQUEST : (uid: a79f6bb1-5eec-43b3-89fd-b4a11dd11d1e) authentication:-> john.doe - [POST] Authentication/SignIn({
  "request": {
    "UserName": "john.doe",
    "Email": "john@doe.it",
    "FirstName": "John",
    "LastName": "Doe"
  }
})
RESPONSE : (uid: a79f6bb1-5eec-43b3-89fd-b4a11dd11d1e) duration:2431ms - null [ERROR: My custom exception for development purposes
System.InvalidOperationException: My custom exception for development purposes
   at ZenProgramming.Chimera.Api.Controllers.AuthenticationController.SignIn(SignInRequest request) in E:\Projects\Chimera\Chimera.Api\Controllers\AuthenticationController.cs:line 76
   at lambda_method(Closure , Object , Object[] )
   at Microsoft.Extensions.Internal.ObjectMethodExecutor.Execute(Object target, Object[] parameters)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeNextActionFilterAsync()]
```