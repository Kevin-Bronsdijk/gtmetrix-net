GTmetrix .Net client
=============
[![Build status](https://ci.appveyor.com/api/projects/status/q5bowvblktu9hs9a?svg=true)](https://ci.appveyor.com/project/Kevin-Bronsdijk/gtmetrix-net) [![NuGet](https://img.shields.io/nuget/v/gtmetrix-net.svg?maxAge=2592000)](https://www.nuget.org/packages/gtmetrix-net/)
***

### GTmetrix .Net client targeting .Net framework version 4.5.2 and higher.
NuGet Package available on [NuGet](https://www.nuget.org/packages/gtmetrix-net/).

GTmetrix-net, a wrapper around the GTmetrix API, offers .Net developers an easy way to utilize GTmetrix's performance testing service. Using the GTmetrix-net, you can integrate performance testing into your development environment or into your application using a simple .net interface.

Within the current version it’s possible to submit test requests and retrieve Test results as well as retrieve Location, Browser and Account status (Account quota’s) information. Additional helpers for downloading resources, Async poll state and the ability to create test sets, will be added in the near future. 

***

* [Getting Started](#getting-started)
* [Installation](#installation)
* [How To Use](#how-to-use)
  * [Authentication](#authentication)
  * [Start a request](#start-a-request)
  * [Get the test results](#get-the-test-results)
  * [Get a list of test locations](#get-a-list-of-test-locations)
  * [Get a list of browsers](#get-a-list-of-browsers)
  * [Download test resources](#Download-test-resources)
  
## Getting Started

First you need to sign up at [gtmetrix.com](https://gtmetrix.com/) and obtain your unique **API Key**. You can generate or view your API key at the API Key box at the top of the [API Details page](https://gtmetrix.com/api/). Once you have set up your account and generated the Key, you can start using the GTmetrix .Net client.

## Installation

To install GTmetrix-net, run the following command in the Package Manager Console
Install-Package gtmetrix-net

## How to use

### Authentication

The first step is to authenticate by providing your unique API Key and Username while creating a new client connection. Use your e-mail address as the username.


```C#
using GTmetrix;
using GTmetrix.Http;

var connection = Connection.Create("Api_Key", "Username");
```

### Start a request

Starting a new Test request is simple. The first step is to create a new `TestRequest` found within the `GTmetrix.Model` namespace. The constructor overloads can be used to override the browser and location defaults. Or assign optional values by using its properties. After having setup your `TestRequest`, call `SubmitTest`, part of the GTmetrix-net client, and make sure to provide the test request.   

```C#
var client = Client(connection);   

var request = new TestRequest(new Uri("http://devslice.net"), 
                Locations.London, Browsers.Chrome);
                
var response = client.SubmitTest(request);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var id = response.result.Body.TestId;
}
```

### Get the test results

Test results can be gathered by calling `client.GetTest("test_id");`. Just like the underlying GTmetrix API, some data might not be present based on the status your test request. In the future a poll helper will be added to eliminate the need to write additional polling code.

Note that the test ID is only valid for 3 days. The GTmetrix report for the URL will be valid for 30 days.

```C#
var response = client.GetTest("Test_Id");
var result = responseCheck.Result;

if (result.StatusCode == HttpStatusCode.OK && result.Body.State == ResultStates.Completed)
{
  var pageLoadTime = result.Body.Results.PageLoadTime;
  ...
}
```

### Get a list of test locations

To retrieve the list with all possible locations you can call `client.Locations()`

```C#
var response = client.Locations();

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    foreach (var item in response.Result.Body)
    {
        var locationId = item.Id;
        var locationName = item.Name;
        ...
    }
}
```

### Get a list of browsers

To retrieve the list with all possible browsers you can call `client.Browsers()`

```C#
var response = client.Browsers();

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    foreach (var item in response.Result.Body)
    {
        var browsersId = item.Id;
        var browsersName = item.Name;
        ...
    }
}
```

### Download test resources

Test resources such as the Har, SpeedTest and Yslow files can be retrieved using `client.DownloadResource("test_id", ResourceTypes.{type})`. The information is exposed as a bytes array and can be accessed as displayed in the example below:

```C#
var response = client.DownloadResource("test_id", ResourceTypes.PageSpeed);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var pageSpeedJson = System.Text.Encoding.UTF8.GetString(resultResource.Body);
}
```
Currently, I’ve only implemented Har, Yslow  and Speedtest. Support for other resource types and retrieving multiple resources at once will be added in the future.








## LICENSE - MIT

Copyright (c) 2016 Kevin Bronsdijk

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
