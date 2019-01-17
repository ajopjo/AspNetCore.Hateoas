# AspNetCore.HypermediaLinks
Repository for ASP.Net Core Hateoas support

# Hateoas support in ASP.NET Core

This package provides support to enable HATEOAS principles to a resource when working with ASP.NET Core.

## Features

- A base model class that will enable a developer to define a resource's relationship to another resource(s).
- Automatic support for link generation when the requested media type is application/hal+json
- A developer can configure to generate link even when the request type is application/json
- Note: work only with json requests.
- links will be generated based on a string template
- links will be generated based on templates defined on the appsettings file.
- Links will be generated based on the controller and action names.

## Quick Start Guide

### Install the nuget package

```csharp
	Package Manager: Install-Package AspNetCore.HypermediaLinks
	CLI: dotnet add package AspNetCore.HypermediaLinks
```

### Add Hypermedia support to a model

This package contains a base class **HyperMediaSupportModel** that allows you to add instances of Link(s) to a model and ensure that it will be rendered properly.

```csharp
	public class TestClass : HyperMediaSupportModel
```
Implement the below method

```csharp
	 public override void AddHypermediaLinks(HypermediaBuilder builder)
```
Inside this method you will define this resource's relationship to itself and other resources. The next section explains how to define a link inside this method.

### Define Link Relations

You can add a resource's link relationship using the *Add* method. The *Add* method takes an input of a link object which will be created using the *HypermediaBuilder* object.

```csharp
	public override void AddHypermediaLinks(HypermediaBuilder builder)
	{
		Add(builder.);
	}
```

The builder supports three approaches to generate links. The builder follows a fluid pattern to generate the link. The below section explains how a link can be created using a HypermediaBuilder object.

#### From Controller and Action methods

If the resources to be linked are defined in the same project, this approach use a type safe way to define the link relationships.

```csharp
    Add(builder.FromController<ControllerName>(c => nameof(c.ActionName), values: new { id = Id, name = Name }).Type("GET").Build().AddSelfRel());
```
If the link type is GET and you want to pass query string parameters, supply the optional input parameter *values:* with an anonymous object. Please note: the field name matches to the field names defined in the Action Method.

Build() will build the link. Prior to that you can use the methods Title, Templated.... to define other link values. 

AddSelfRel() adds a "self" relationship. If you want a named relationship use the method AddRel("name").

#### From a string template

If the resources to be linked are not defined in the same project, you can use a templated approach or a configuration approach. 

```csharp
	 public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(builder.Fromtemplate("/moq/{id}/items", new { id = Id }, uri: new Uri("https://templatetest.com")).Type("GET").Build().AddSelfRel());
        }
```

if the optional input parameter *uri* is not specified, the incoming HTTP request's scheme and host name is used to create uri.


#### From appsettings file (configuration)

This option will avoid hardcoding of link templates defined inside the code. To use this option:

1. Define the below config section, hyperMediaLinks, inside the appsettings file
```json
	"hyperMediaLinks": [
    {
      "name": "modeltest",
      "type": "GET",
      "path": "api/modeltest/{id}",
      "uri":  "https://configtest.com"
    }
  ]
``` 
*name* field is used to uniquely identify the name of the link
*path* is used to define the url path and route field. The values to the route fields are defined using an anonymous object.
*uri* field is optional. If you don't supply any values, the incoming request's hostname and schema is used.
 
```csharp
	 public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(builder.FormConfiguration("modeltest", new { id = 1 }).Build().AddSelfRel());
        }
```

#### Example

```csharp
     public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(builder.FormConfiguration("modeltest", new { id = 1 }).Build().AddSelfRel());

             Add(builder.Fromtemplate("/moq/{id}/items", new { id = Id }, uri: new Uri("https://templatetest.com")).Type("GET").Build().AddSelfRel());

              Add(builder.FromController<ControllerName>(c => nameof(c.ActionName), values: new { id = Id, name = Name }).Type("GET").Build().AddSelfRel());
        }
```

### Add Service Dependency to the Start Up

```csharp
services.AddLinkBuilder(Configuration, true);
```
- if the links are defined inside the appsettings file, pass the optional configuration field.
- if the links need to be generated for the media type *application/json*, pass the flag true.

## Additional Comments

- If you put the service behind a proxy server or API management tool, use X-FORWARDED-HOST and *-PROTO headers from the proxy
