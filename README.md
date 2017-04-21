# Yuki
At this point **Yuki** is a very basic time tracking API. It has no authentication nor authorization yet and is very much experimental but it *does* already offer some reasonably valuable functionality in a simple way.

Not that as always, this is also very much a *sandbox* project to experiment.

# Workspaces
A **workspace** is a container for all the things that happen inside the time tracking system. It's a unit of time-tracking entries, timers, users, projects and all that is required to track time.

# Timers
As a user you can start and stop timers. A timer runs in a **workspace** so that **entries** that result from that timer end up in that workspace as well. A user can only start a single timer. You'll have to stop the current timer first before you can start a new one.

# Projects
Within a **workspace** you can have zero or more projects. Projects provide for an easy way to keep track of related tasks.

# Tags
Any entry can be tagged with as many **tags** as you want. Tags don't care about hierarchy or subdivision and provide an easy way to slice your time tracking data any way you want. Tags are local to a workspace.

# Entries
Once you stop a **timer** this results in an **entry** which hold the duration and other related information such as project and tags.

Note that entries can be created easily without any meta-information and it's easy to update existing entries with that information later on. This means you can just focus on work and worry about bookkeeping later.