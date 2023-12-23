# KoreCache
[Kore Cache](https://kore-tools.com) wrapper library implementation for C#.

# Integration tests
The integration tests expect one of two things:

1. Environment variable ``KoreCacheApiKey`` that is set to the API key to be used during the integration test.
2. Create an ``env.local`` file in the root of the integration project, set its Build Action to "Copy always", on the first line put ``KoreCacheApiKey=REPLACEAPIKEY`` and replace with your actual API key, then rebuild the integration tests and run them.

Either option works, though the environment variable takes precendence over the ``env.local`` file.
