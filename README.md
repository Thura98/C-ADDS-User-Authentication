# C-Sharp--ADDS-User-Authentication
Code for Adds User Authentication located in C-Sharp--ADDS-User-Authentication/TestAPI/TestAPI/Controllers/ValuesController.cs
        
        
        [HttpGet]
        [HttpPost]
        public bool IsAuthenticated(string username, string pwd)
        {
            string domainAndUsername = "16ELM" + @"\" + username;
            //_path = "LDAP://172.20.129.73/dc=16elm,dc=local";
            _path = "LDAP://192.168.64.128/dc=16elm,dc=local";
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
            try
            {
                // Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                // Update the new path to the user in the directory
                _path = result.Path;
                _filterAttribute = (String)result.Properties["cn"][0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error authentication user. " + ex.Message);
            }
            return true;
        }
