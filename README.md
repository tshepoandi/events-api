GET /api/users: Get all users
GET /api/users/{id}: Get a specific user
POST /api/users: Create a new user
PUT /api/users/{id}: Update an existing user
DELETE /api/users/{id}: Delete a user

## Examples

user- post request example:
{

<!-- id is not mandatory for neither -->

"id": 5,

<!-- username and email are mandatory for post requests -->

"username": "moloko",
"email": "moloko@email.com",

<!-- notmandatory but needed for update requests -->

"createdEvents": null,  
"attendingEvents": null,
"reviews": null,
"comments": null
}
