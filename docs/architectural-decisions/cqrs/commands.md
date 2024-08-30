# Commands
- will modify the state of the system - [source](https://en.wikipedia.org/wiki/Command_Query_Responsibility_Segregation)
- cannot call another command from a command 
	- because it might create circular dependencies - [source](https://medium.com/workleap/why-command-should-not-call-command-in-cqrs-5da046a9fed1)
	- because it breaks the CQRS pattern due to the potential issues with complexity, coupling, and transaction management - which is detailed below
	
## Why a command should not directly call another command 

In the context of the CQRS (Command Query Responsibility Segregation) pattern and the use of MediatR, calling a command inside another command handler is allowed, but whether it is recommended depends on several considerations. 
A good resource is Jimmy Bogard, the creator of MediatR, who talks about this [here](https://lostechies.com/jimmybogard/2015/05/05/cqrs-with-mediatr-and-automapper/)

Let's break this down:

### CQRS Pattern Principles
- Separation of Concerns: CQRS emphasizes separating the read model (queries) from the write model (commands) to handle commands and queries differently, often using separate models and possibly even different storage. Each command should represent a distinct action or intent.
- Single Responsibility Principle: Each command handler should ideally handle one specific task or unit of work. The aim is to keep command handlers small and focused on a single responsibility.

### MediatR and Command Nesting
MediatR is a library that facilitates the mediator pattern, promoting decoupling between request senders and handlers. While MediatR itself allows for commands to be called from within other commands, this flexibility can lead to practices that may or may not align with the pure intentions of the CQRS pattern.

#### Pros and Cons of Nesting Commands
##### Pros
- Reusability: Sometimes, one command might logically consist of several other operations that are represented by different commands. Nesting can promote reuse of these operations.
- Consistency: If different operations require the same action (e.g., sending a notification), nesting commands can ensure this action is performed consistently.
- Modularity: Breaking complex operations into smaller commands can enhance modularity and make each component easier to test and maintain.

##### Cons
- Complexity: Nesting commands can lead to complex execution paths that are harder to understand, debug, and maintain.
- Coupling: Nesting commands may lead to tighter coupling between different parts of the system, which contradicts the CQRS philosophy of keeping command handling separate and distinct.
- Transaction Management: Nesting commands can complicate transaction management. If one command fails, the handling of compensating transactions or rollbacks can become tricky.
- Hidden Side Effects: When commands are called within other commands, it can lead to hidden side effects that are not immediately apparent, making the system's behavior less predictable.

### Best Practices
- Use Events Instead of Direct Command Calls: A more CQRS-aligned approach is to use domain events to trigger actions instead of directly calling commands within other commands. For example, a OrderCreatedEvent can be published after an order is created, and a separate event handler can listen for this event and then issue a NotifyCustomerCommand.
- Keep Commands Small and Focused: Adhere to the single responsibility principle by keeping each command handler focused on one specific unit of work.
- Use Orchestration: If a command needs to trigger multiple other commands, consider using an orchestration service or process manager to handle the sequence of command calls. This keeps individual command handlers simple and focused.
- Transaction Boundaries: Define clear transaction boundaries and manage them carefully. Ensure that nested commands either all succeed or all fail to maintain data consistency.

### Conclusion
While calling a command inside another command is technically allowed when using MediatR, it is not always recommended by the CQRS pattern due to the potential issues with complexity, coupling, and transaction management. Instead, consider using events or orchestration mechanisms to maintain the benefits of separation of concerns, simplicity, and scalability that CQRS is designed to provide.
	
