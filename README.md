# DDD-CQRS-Template
Template project for showing how to implement the CQRS pattern in a Domain-Driven Design fashion with .Net 7.

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
- `Commands` holds all Commands that can be sent using the CQRS pattern, these must be handled exactly once by the core domain. Each file contains the command and its handler with `file` visibility, as we'll never need direct access to the handlers (not even for unit and integration testing)
- `Notifications` doesn't actually exist in this template, but create it if you need to broadcast notifications about your aggregates. An example could be broadcasting whenever something gets persisted, so anyone in need of the updated version knows about it. (e.g. for Cache Invalidation)
- `Queries` holds all the queries that can be sent using the CQRS pattern, these must be handled exactly once by the core domain. Again each file contains the handler with `file` visibility.
- `Repositories` are merely slightly more concrete interfaces based on the abstract repository interfaces.
- `ValueObjects` holds all Value Objects for Todos, in this example only the `TodoId` exists.

## Infrastructure
This is where your concrete infrastructure implementations go, in this template it's merely the single repository. Dependency Injection is used in `Setup.cs` as a simple way for the API to inject the infrastructure. This is part of the Onion Architecture and allows you to easily swap out this layer.


# Understanding the template
I'd suggest reading the code in a top-down fashion starting with the API and following the code trail down into the domain and finally infrastructure layer. This should give you a good understanding of both what is going on as well as _why_.

# What's still missing in the template
One thing I realized is missing here, is cross sub-domain communication. This is obviously because there's just one sub-domain: `Todos`.
When doing cross sub-domain communication you should rely on `CQRS`, as well as `Mediator notifications` for any pub-sub type of communication. This way avoids tightly coupling of sub-domains, as all the communication relies on the `Command/Query/Notification` "contracts".
