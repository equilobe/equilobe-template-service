# Intro

In order to support patch endpoints we standardized our implementation to follow the pattern described in [ASP.NET Core JsonPatch](https://learn.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-8.0).

JSON Patch is a format defined in [RFC 6902](https://datatracker.ietf.org/doc/html/rfc6902) that allows partial updates to a JSON document. It specifies a sequence of operations (e.g., add, remove, replace, copy, move, and test) to be applied to the target resource.

Before adding your first patch endpoint it would be useful to understand the following:
- [possible operations](https://learn.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-8.0#operations)
- [example](https://learn.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-8.0#resource-example)

# Patch endpoint manifesto

In essence the patch endpoint is an update of the resource which is done in a more granular way, without having to send the entire resource, as you would have to do in a put endpoint. In other words you're solely sending the fields which you want to update for a given resource.  

Keeping this in mind, our example assumes that you would most likely always have a put endpoint, and this patch endpoint builds on it by loading the resource, applying the patch and then reusing the already in place put logic. It comes with the drawback that it would require to load the resource two times, but we consider this as being negligible. 

This is not the only way the patch endpoint can be implemented, but our example can be adapted to other situations, situations where you might not have a put endpoint already implemented. 

# How to use in a frontend application 

For frontend client applications the de facto seems to be `fast-json-patch`. Below you can find an example on how to build the payload required by the patch endpoint. 

```javascript 
const { compare } = require('fast-json-patch');

// Example usage:
const oldObj = { name: 'Alice', age: 30, address: { city: 'New York' }, kids: ['Bob', 'Carol'] };
const modifiedObj = { name: 'Alice', age: 32, kids: ['David'] };

const patch = compare(oldObj, modifiedObj);
console.log('Patch:', patch);