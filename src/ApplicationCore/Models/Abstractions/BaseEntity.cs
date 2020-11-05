// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.Models.Abstractions
{
    // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
    // Using non-generic integer types for simplicity and to ease caching logic
    public abstract class BaseEntity
    {
        public virtual int Id { get; protected set; }
    }
}
