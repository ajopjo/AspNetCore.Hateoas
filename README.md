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

This package contains a base class HyperMediaSupportModel that allows you to add instances of Link to a model and ensure that it will be rendered properly.

```csharp
	public class TestClass : HyperMediaSupportModel
```
Implement the method

```csharp
	 public override void AddHypermediaLinks(HypermediaBuilder builder)
```
Inside this method you will define this resource’s relationship to itself and other resources. The next section explains how to define a link inside this method.

### Define Link Relations

You can add a link relation ship using the Add method. Add method takes an input of a link object which will be created using the HypermediaBuilder object.

```csharp
	public override void AddHypermediaLinks(HypermediaBuilder builder)
	{
		Add(builder.);
	}
```

The builder supports three approaches to generate links. The builder follows a fluid pattern to generate the link. The below section explains how a link can be created using a HypermediaBuilder object.

#### From Controller and Action methods

If the resources to be linked are defined in the same project, this apparoach use a type safe way to define the link relationships.

```csharp
    Add(builder.FromController<ControllerName>(c => nameof(c.ActionName), values: new { id = Id, name = Name }).Build().AddSelfRel());
```
If the link type is GET and you want to pass query string parameters, supply the optional input parameter *values:* with an anonymous object. Please note: the field name matches to the field names defined in the Action Method.

Build() will build the link. Prior to that you can use the methods Title, Templated.... to define other link values. 

AddSelfRel() adds a "self" relationship. If you want a named relationship use the method AddRel("name").

#### From a string template

If the resources to be linked are not defined in the same project, you can use a templated approach or a configuration approach. 

```csharp
	 public override void AddHypermediaLinks(HypermediaBuilder builder)
        {
            Add(builder.Fromtemplate("/moq/{id}/items", new { id = Id }, uri: new Uri("https://templatetest.com")).Build().AddSelfRel());
        }
```

if the optional input parameter *uri* is not specified, the incoming HTTP request's scheme and host name is used to create uri.


### From appsettings file (configuration)


## Additional Comments

- If you put the service behind the proxy, use X-FORWARDED-HOST and -PROTO headers.
