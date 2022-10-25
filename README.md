
# Communion API

This API was build for the Communion Forum.
It will control and manage the users, categories, threads, and profiles of the project.







### What can it do right now?
As of now, it can register users to the Database.

### Registering Users

To register users, send an Http Request with the required parameters.
They are as follow:
- Username
- Password
- Name
- Email
If you are trying to register with an existing username, a bad request will be sent back,
and the registering process will halt.