# TL Test solution

## What is this?

This solution contains four projects and their tests

* TL.XR *the main ASP.NET Core 3.1 API project*. This project contains a single controller `ConvertController`, two entry points (one for local deployment and one for AWS Lambda), and the `Startup.cs` file.
* TL.XR.Infrastructure *contains interface and domain model definitions*
* TL.XR.Core *contains the bulk of business logic implementation*
* TL.XR.TLExchangeRatesProvider *contains the implementation of communicating with the TL exchange rates provider*

All the .Tests projects contain xUnit tests for the corresponding projects.

## Running the project
There is a deployed version of the solution on AWS Lambda that can be found at `https://7o33vk02q1.execute-api.eu-west-2.amazonaws.com/Prod/`. 

```
curl --request GET \
  --url 'https://7o33vk02q1.execute-api.eu-west-2.amazonaws.com/Prod/convert/gbp?targetcurrency=usd&amount=1000'
```

The solution can be compiled and deployed locally using the dotnet command line or Visual Studio

There is only one controller action at the path `convert/{sourceCurrency}?targetCurrency={ISOCode}&amount={amount}`

If the conversion is successful, the API will return a `200 OK` response with the corresponding payload as below:
```
{
  "from": {
    "currency": "gbp",
    "amount": 1000
  },
  "to": {
    "currency": "usd",
    "amount": 1384.935000
  },
  "errors": []
}
```

If there is an error, the API will return a `412 Precondition Failed` response and some errors in the errors array like below:

```
{
  "from": {
    "currency": "gbpa",
    "amount": 1000
  },
  "to": {
    "currency": "usd",
    "amount": 0
  },
  "errors": [
    "UNABLE_TO_CONVERT"
  ]
}
```