# Yuki
At this point **Yuki** is a very basic time tracking API. It has no authentication nor authorization yet and is very much experimental but it *does* already offer some reasonably valuable functionality in a simple way.

Not that as always, this is also very much a *sandbox* project to experiment.

# Concepts
## Workspaces
A **workspace** is a container for all the things that happen inside the time tracking system. It's a unit of time-tracking entries, timers, users, projects and all that is required to track time.

## Timers
As a user you can start and stop timers. A timer runs in a **workspace** so that **entries** that result from that timer end up in that workspace as well. A user can only start a single timer. You'll have to stop the current timer first before you can start a new one.

## Projects
Within a **workspace** you can have zero or more projects. Projects provide for an easy way to keep track of related tasks.

## Tags
Any entry can be tagged with as many **tags** as required. Tags don't care about hierarchy or subdivision and provide an easy way to slice your time tracking data any way you want. Tags are local to a workspace.

## Entries
Once you stop a **timer** this results in an **entry** which hold the duration and other related information such as project and tags.

Note that entries can be created easily without any meta-information and it's easy to update existing entries with that information later on. This means you can just focus on work and worry about bookkeeping later.

# Gettings started
You should be able to run `update-database` and just use EF migrations to get yourself a local database. After starting up the HTTP service (API) you should be able to execute the commands below. 

Note that by default, a `Default` workspace and a `Foo Bar` user are created. Both have an `Id` value of `1` and we'll work with that for the remainder of this tutorial.

## Getting a list of workspaces
One of the first things you might want to do is get a list of workspaces. Using **PowerShell** this is very easy to do:

    Invoke-WebRequest http://<yuki_endpoint>/api/workspaces

As a small tip, if you **query** you'll probalby get back **JSON** results and using **PowerShell** you can deal with them in a much nicer way:

    (iwr http://<yuki_endpoint>/api/workspaces).content | convertfrom-jsonj | ftf

Unless you did some experimenting this should return a single workspace named `Default`.

## Creating a new workspace
This is very easy using PowerShell and a bit of JSON:

    $uri = 'http://<yuki_endpoijnt>/api/workspaces'
    $body = '{"name": "frotz"}';
    Invoke-WebRequest -Uri $uri -Method POST -ContentType 'application/json' -Body $body

The snippet above will create a new workspace called **frotz**.