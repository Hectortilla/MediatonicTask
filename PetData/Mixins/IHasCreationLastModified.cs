using System;

namespace PetData.Mixins
{
    interface IHasCreationLastModified
    {
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
