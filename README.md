

On package manager console, create a Add-Migration and give it a name
PM> Add-Migration InitialMigration
Next, apply the migration to the database to create the schema.
PM> update-database


A service responding to a http methods is just a web api
A REST api is much more. It is NOT a standard but it is an architectural style
We use standard to implement REST

REST is protocol agnostic  (http or JSON is theoretically not related to REST at all)

Message body = Payload

Richardson Maturity Model
Level 0:
RPC style implementations e.g. SOAP/WCF etc. are XML based and use http ONLY as a transport protocol
Level 1:
HTTP methods are not correectly being used e.g. only POST method is used for everything
Level 2:
HTTP verbs (Get, Put, Post, Delete) are used as intended by the protocol
Correct status codes are used (200, 201, 404 etc.)
Level 3:
Hypermedia is used
API supports HATEOAS - Hypermedia as the engine of application state
-> this introduces discoverability and self-documentation



