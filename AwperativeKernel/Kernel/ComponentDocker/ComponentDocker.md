# Component Docker

### Code and Documentation by Avery Norris

---

One major and neccessary requirement of an **EC/ECS** [(Entity Component System)](https://en.wikipedia.org/wiki/Entity_component_system) is **Nested-Components**; 
However a common problem that can occur during implementation is the redundancy and different types of nesting required.

For instance, let's take **Awperative's** own **ECS** for example. We have two objects that need to be able to hold **Children-Components**

    Scenes

    Components

Originally, both classes carried methods to store **Components** as children; However this was extremely redundant, and **Component-Docker** was created to handle
this in an organised fashion!

## Usage

Called a **Docker** for short; the **Component-Docker** carries **three main functionalities**

    Storing Children Component

    Providing an Interfacet to Modify Children

    Transferring Events to the appropriate Components

