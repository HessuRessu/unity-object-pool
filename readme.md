# Lightweight Object Pooling for Unity

Lightweight, type-safe object pooling system for Unity with extensible poolable behaviours.

This package provides a small but robust pooling architecture designed for runtime-heavy projects where:
- GC allocations must be avoided
- Large numbers of transient objects are spawned
- Object lifetime must be deterministic
- Pool usage should be type-safe and easy to reason about

The system focuses on clarity, explicit ownership, and extensibility rather than large feature sets.

## Compatibility

This package has been implemented and tested with **Unity v6000.0** and **Universal Render Pipeline v17.0.4**.

## Features

- Centralized `PoolProvider` singleton
- Type-safe pooling using generic accessors
- Lifetime-based auto return
- No reflection
- No string or enum keys
- No per-frame allocations
- Extensible `BasePoolable` behaviour model
- Built-in library of common poolable wrappers

## Installation (UPM)

Add this package through your project's `manifest.json`:

```json
{
  "dependencies": {
    "com.pihkura.unity-object-pool": "https://github.com/HessuRessu/unity-object-pool.git"
  }
}
```

Or add via Package Manager:

1. Open **Unity → Window → Package Manager**  
2. Click **+ → Add package from git URL**  
3. Paste:  
   `https://github.com/HessuRessu/unity-object-pool.git`

## Core Concept

Each pooled object derives from `BasePoolable`.

Objects are requested using:

```csharp
var obj = PoolProvider.Instance.Get<MyPoolableType>(context);
```

Returned using:

```csharp
obj.Return(false);
```

The pool key is the **concrete C# type**, ensuring:
- Compile-time safety
- No runtime identifiers
- No accidental cross-pooling

## PoolableContext

Borrow requests use a lightweight context struct:

```csharp
var context = PoolableContext.WithInfinity(
    position,
    rotation
);
```

Or with finite lifetime:

```csharp
var context = PoolableContext.WithFinity(
    1.5f,
    position,
    rotation
);
```

Context contains:
- Position
- Rotation
- Scale
- Optional parent
- Lifetime

Negative lifetime means infinite.

## Minimal Usage Example

```csharp
var line = PoolProvider.Instance.Get<PoolableLineRenderer>(
    PoolableContext.WithInfinity(Vector3.zero, Quaternion.identity)
);
```

Return when no longer needed:

```csharp
line.Return(false);
```

## BasePoolable Responsibilities

`BasePoolable` handles:
- Lifetime tracking
- Context storage
- Provider communication

Derived classes are responsible for:
- Activating GameObject in `OnBorrowed()`
- Deactivating GameObject in `OnReturned()`

This design allows fade-outs, tweens, and custom exit behaviour.

## Built-in Library Poolables

The package includes several ready-to-use poolable wrappers:

- `PoolableGameObject`
- `PoolableLineRenderer`
- `PoolableParticleSystem`
- `PoolableAudioSource`
- `PoolableSpriteRenderer`
- `PoolableMeshRenderer`
- `PoolableTrailRenderer`
- `PoolableLight`

These depend only on Unity components and can be used directly.

## Creating Custom Poolables

Derive from `BasePoolable`:

```csharp
public sealed class MyCustomPoolable : BasePoolable
{
    public override void OnBorrowed()
    {
        gameObject.SetActive(true);
    }

    public override void OnReturned()
    {
        gameObject.SetActive(false);
    }

    public override void OnUpdate(float deltaTime)
    {
    }
}
```

Add the prefab to `PoolProvider.presets`.

## Architecture Overview

```
PoolProvider
 ├─ Dictionary<Type, Stack<BasePoolable>>
 └─ Factory Presets

BasePoolable
 ├─ Borrow / Return
 ├─ Lifetime
 └─ Derived Poolables
```

Design goals:
- Explicit ownership
- Deterministic lifetime
- Easy debugging
- Predictable behaviour

## Performance Characteristics

- No allocations during borrow/return
- O(1) average stack access
- Suitable for large numbers of transient objects

## Limitations (By Design)

- No editor tooling
- No built-in visual debugging
- No addressable integration

These can be layered on top without changing core architecture.

## License

MIT
