angular.module("FLMClientApp.config", [])
.constant("EnvConfig", {"showDebugInfo":true,"api":"http://localhost:5000/","auth":{"server":"https://localhost:44335/","clientID":"flm.client.angular","requestedScopes":["openid","flm.api"]},"defaultItemsPerPage":10});
