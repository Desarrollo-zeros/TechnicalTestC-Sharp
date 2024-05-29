using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Contracts.Base;


namespace Domain.Base
{
    /// <summary>
    /// Base class for entities with a generic primary key.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key.</typeparam>
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        /// Gets the primary key as an object.
        /// </summary>
        /// <returns>The primary key.</returns>
        public object? GetId()
        {
            return Id;
        }

        /// <summary>
        /// Gets the primary key as an array of objects.
        /// </summary>
        /// <param name="id">The primary key.</param>
        /// <returns>An array containing the primary key.</returns>
        public object[] GetKeys(object? id) => new object[] { id };

        public object[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}
