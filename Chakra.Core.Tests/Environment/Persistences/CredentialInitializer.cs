using System;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Persistences;
using ZenProgramming.Chakra.Core.Persistences.Attributes;
using ZenProgramming.Chakra.Core.Utilities.Security;

namespace ZenProgramming.Chakra.Core.Tests.Environment.Persistences
{
    /// <summary>
    /// Initializer for credential
    /// </summary>
    [PersistenceInitializer]
    public class CredentialInitializer : PersistenceInitializerBase<Credential>
    {
        /// <summary>
        /// Initialize elements
        /// </summary>
        /// <param name="elements">Empty elements to initialize</param>
        protected override void Initialize(IList<Credential> elements)
        {
            //Validatione argomenti
            if (elements == null) throw new ArgumentNullException(nameof(elements));

            //Accodo gli elementi
            elements.Add(new Credential { Key = "maurob@icubed.it", Password = ShaProcessor.Sha1Encrypt("P4ssw0rd") });
            elements.Add(new Credential { Key = "davidem@icubed.it", Password = ShaProcessor.Sha1Encrypt("P@ssw0rdDavide") });
            elements.Add(new Credential { Key = "lucaq@icubed.it", Password = ShaProcessor.Sha1Encrypt("P@ssw0rd4") });
        }
    }
}
