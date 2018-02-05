﻿using System;
using FuturisX.Helpers.Exception;

namespace FuturisX.Domain.Entities
{
    /// <summary>
    /// Used to identify an entity.
    /// Can be used to store an entity <see cref="Type"/> and <see cref="Id"/>.
    /// </summary>
    [Serializable]
    public class EntityIdentifier
    {
        /// <summary>
        /// Entity Type.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Entity's Id.
        /// </summary>
        public object Id { get; private set; }

        /// <summary>
        /// Added for serialization purposes.
        /// </summary>
        private EntityIdentifier()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityIdentifier"/> class.
        /// </summary>
        /// <param name="type">Entity type.</param>
        /// <param name="id">Id of the entity.</param>
        public EntityIdentifier(Type type, object id)
        {
            Throw.IfArgumentNull(type, nameof(type));
            Throw.IfArgumentNull(id, nameof(id));

            Type = type;
            Id = id;
        }
    }
}