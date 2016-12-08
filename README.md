# Yuki
This is a tool to migrate your databases in an efficient and safe way. It is
very heavily inspired by the `RoundhousE` migration tool.

# Script types
* `onetime` scripts are only executed one time and cause the migration to fail
when the system detects any changes after already running them.
* `anytime` scripts are run any time they have changes.
* `everytime` scripts are run every migration run.