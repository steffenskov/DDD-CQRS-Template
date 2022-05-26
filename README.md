# DDD-CQRS-EventSourcing-Template
Template project for showing how to implement the CQRS pattern in a Domain-Driven Design fashion with .Net 6.

Furthermore Onion Architecture is applied as well, to enable easy replacement of infrastructure code without changes to the external API or Domain logic.

MediatR is used for the CQRS pattern for both issuing commands and queries.
If you don't want to use MediatR you can build domain services that mimics the functionality instead.

The template application provides a CRUD API for dealing with todos and is split into 3 .Net projects:
- `Api`
- `Domain`
- `Infrastructure`

Finally a rudimentary grasp of the following concepts will be required to gain the most benefit from this template:
- [Domain-Driven Design](https://en.wikipedia.org/wiki/Domain-driven_design)
- [CQRS](https://en.wikipedia.org/wiki/Command%E2%80%93query_separation)
- [Onion Architecture](https://en.everybodywiki.com/Onion_Architecture)
- [MediatR](https://github.com/jbogard/MediatR/wiki)

## Api
This project holds your WebAPI controller as well as any models sent and received to/from the user.
The controller mostly boils down to creating the proper `Command` or `Query` and sending it off using `MediatR`.

## Domain
Here's where all the magic happens. The project is sorted into 3 main folders:
- `Exceptions` holds any custom Exceptions we'll be using
- `Abstractions` holds all the generic interface definitions to be used across AggregateRoots. This *could* also be split out into a separate project, like e.g. a `Shared Kernel`.
- `Todos` holds everything related to our `Todo` **AggregateRoot**

Beneath both `Abstractions` and `Todos` are a bunch of new folders, I'll describe them from the `Todo` perspective, as they're more concrete version of the generic ones beneath `Abstractions`:
- `Aggregates` holds the AggregateRoot (`Todo`) as well as any child aggregates (none in this simple template)
- `Commands` holds all Commands that can be sent using the CQRS pattern, these must be handled exactly once by the core domain. (this is ensured by using the `Send` method of `MediatR` as opposed to `Publish`)
- `Queries` holds all the queries that can be sent using the CQRS pattern, these must be handled exactly once by the core domain. (this is ensured by using the `Send` method of `MediatR` as opposed to `Publish`)
- `Repositories` are merely slightly more concrete interfaces for the datasource.


## Infrastructure
This is where your concrete infrastructure implementations go, in this template it's merely the datasource repository. Dependency Injection is used in `Setup.cs` as a simple way for the API to inject the infrastructure. This is part of the Onion Architecture and allows you to easily swap out this layer.


# Understanding the template
I'd suggest reading the code in a top-down fashion starting with the API and following the code trail down into the domain and finally infrastructure layer. This should give you a good understanding of both what is going on as well as *why*.


# What's still missing in the template
One thing I realized is missing here, is cross AggregateRoot communication. You *may* argue this seldomly occurs, and that one of the two roots should maybe be a child aggregate of the other.
However I find it occurs often enough to warrant adressing - I'll circle back to that in an update in the future. 
For now here's the brief explanation: Use CQRS when going across AggregateRoots as well. This way an AggregateRoot doesn't rely on e.g. a repository of the other AggregateRoot, but rather merely a query or command.
