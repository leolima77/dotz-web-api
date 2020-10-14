# Dotz WebApi Test

- Change string connection at appsettings.json (Dotz.WebApi and Dotz.Data)
- On Packager Manager Console run "Update-Database --verbose"
- Create a language request for example POST /Language/Add
	<pre>
	{
  "name": "pt-BR",
  "culture": "pt-BR",
  "id": "ac41304a-0074-49c3-9d1c-cdcdd827bc56",
  "createTimestamp": "2020-10-14T05:00:00.787Z",
  "creator": "235db857-86d6-47a3-b282-6142eac4112b",
  "status": 1
}
	</pre>
	
- Create an user POST /User/Add
	<pre>
	{
  "emailConfirmed": true,
  "email": "leolima77@gmail.com",
  "userName": "leolima77",
  "password": "1@qwasZX",
  "phoneNumber": "11991792129",
  "phoneNumberConfirmed": true,
  "title": "sr",
  "userRoles": [
    {
      "id": "7b620f68-19dd-4d8b-b1dd-f274dc12960f",
      "userId": "a2aa0177-a736-46eb-a8e3-f0607a7db388",
      "role": {
        "description": "role 1",
        "createTimestamp": "2020-10-12T01:18:07.709Z",
        "creator": "235db857-86d6-47a3-b282-6142eac4112b",
        "status": 1,
        "id": "4cedb2cc-d29e-4368-b508-25012e695e78",
        "name": "role-1",
        "normalizedName": "string",
        "concurrencyStamp": "string"
      },
      "roleId": "c52a2ba8-81a5-4cbc-93ca-e6ecf7cfb118"
    }
  ],
  "languageId": "ac41304a-0074-49c3-9d1c-cdcdd827bc56",
  "id": "a2aa0177-a736-46eb-a8e3-f0607a7db388",
  "createTimestamp": "2020-10-11T19:51:18.394Z",
  "creator": "235db857-86d6-47a3-b282-6142eac4112b",
  "status": 1
}
</pre>

- Get token bearer on POST /Authentication/LoginAsync with user created previously