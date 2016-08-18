GTmetrix .Net client
=============
[![Build status](https://ci.appveyor.com/api/projects/status/q5bowvblktu9hs9a?svg=true)](https://ci.appveyor.com/project/Kevin-Bronsdijk/gtmetrix-net) [![NuGet](https://img.shields.io/nuget/v/gtmetrix-net.svg)](https://www.nuget.org/packages/gtmetrix-net/)
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
  * [Submit a test request](#submit-a-test-request)
  * [Simple request](#simple-request)
  * [Async request](#async-request)
  * [Retrieve the test results](#retrieve-the-test-results)
  * [Manually polling](#manually-polling)
  * [Async](#async)
  * [Get a list of test locations](#get-a-list-of-test-locations)
  * [Get a list of browsers](#get-a-list-of-browsers)
  * [Download test resources](#download-test-resources)
  
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

### Submit a test request
There are two ways of submitting a test request; The first one works similar to the underlying GTmetrix API and will return a test id. This id can be used to retrieve the test results at a later time. The second option is a async call which will return as soon as your test results are available. Let’s take a look at both options.

### Simple request

Submitting a new Test request is simple. The first step is to create a new `TestRequest` found within the `GTmetrix.Model` namespace. The constructor overloads are helpers and can be used to override the browser, location and connection speed defaults. Or assign optional values by using its properties. After having setup your `TestRequest`, call `SubmitTest`, part of the GTmetrix-net client, and make sure to provide the test request.   

```C#
var client = Client(connection);   

var request = new TestRequest(
    new Uri("http://devslice.net"),
    Locations.London,
    Browsers.Chrome)
    {
        // Optional settings
        EnableAdBlock = true, 
    };                             
                
var response = client.SubmitTest(request);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var id = response.result.Body.TestId;
}
```

### Async request

By using the Async version you don’t have to deal with separate calls for submitting and retrieving your test. This is especially helpful when you want to results as soon as available.

```C#
var client = Client(connection);   

var request = new TestRequest(
    new Uri("http://devslice.net"),
    Locations.London,
    Browsers.Chrome)
    {
        // Optional settings
        EnableAdBlock = true, 
    };                             
                
var response = client.SubmitTestAsync(request);

if (response.Result.StatusCode == HttpStatusCode.OK && 
    response.Result.Body.State == ResultStates.Completed)
{
  var pageLoadTime = response.Result.Body.Results.PageLoadTime;
  ...
}
```

You can also provide the raw API values for browser, location and connection speed if desired. The possible values are available within gtmetrix's API docs.

```C#
new TestRequest(new Uri(TestData.TestWebsite))
{
    Browser = 3,
    ConnectionSpeed = "5000/1000/30",
    Location = 2
};
```

### Retrieve the test results

There are two possible ways to retrieve the test results; manually polling or awaiting the call using the async version.

Note that the test ID is only valid for 3 days. The GTmetrix report for the URL will be valid for 30 days.

### Manually polling

Test results can be gathered by calling `client.GetTest("test_id");`. Just like the underlying GTmetrix API, some data might not be present based on the status your test request (Queued, Started, Completed, Error). Therefore, you will have to manually poll until the status has changed to Completed or Error. 

```C#
// poll loop logic
var response = client.GetTest("Test_Id");
var result = responseCheck.Result;

if (result.StatusCode == HttpStatusCode.OK && result.Body.State == ResultStates.Completed)
{
  var pageLoadTime = result.Body.Results.PageLoadTime;
  ...
}
// poll loop logic
```

### Async

The Async version eliminates the need to poll manually. By default, the API will internally check if the test results are available. At the moment polling will be performed internally for a maximum of 10 times with 2 a second wait period between each poll.

```C#
var response = client.GetTestAsync(result.Body.TestId);
var result = response.Result;

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

Saving a file to disk is simple as can be seen in the example below;

```C#
var response = client.DownloadResource("test_id", ResourceTypes.PageSpeed);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    File.WriteAllBytes("c:\\test\\PageSpeed.json", responseResource.Result.Body);
}
```

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
