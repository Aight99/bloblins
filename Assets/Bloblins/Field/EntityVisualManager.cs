using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityVisualManager
{
    private readonly Transform parent;
    private readonly GameObject creatureVisualPrefab;
    private readonly GameObject itemVisualPrefab;
    private readonly AnimationQueue animationQueue;
    private readonly float moveSpeed;
    private readonly Func<IEnvironmentObject, Vector3> getWorldPositionForObject;

    private readonly Dictionary<CellPosition, GameObject> entityVisuals =
        new Dictionary<CellPosition, GameObject>();

    public EntityVisualManager(
        Transform parent,
        GameObject creatureVisualPrefab,
        GameObject itemVisualPrefab,
        AnimationQueue animationQueue,
        float moveSpeed,
        Func<IEnvironmentObject, Vector3> getWorldPositionForObject
    )
    {
        this.parent = parent;
        this.creatureVisualPrefab = creatureVisualPrefab;
        this.itemVisualPrefab = itemVisualPrefab;
        this.animationQueue = animationQueue;
        this.moveSpeed = moveSpeed;
        this.getWorldPositionForObject = getWorldPositionForObject;
    }

    public void UpdateVisuals(FieldState fieldState, Action<CellPosition> onCellClicked)
    {
        var oldPositions = BuildOldPositionsMap();
        var movedObjects = DetectMovedObjects(fieldState, oldPositions);

        AnimateMovedObjects(movedObjects);
        CreateNewEntityVisuals(fieldState, onCellClicked);
        RemoveDestroyedEntityVisuals(fieldState);
    }

    public void ClearAll()
    {
        foreach (var visual in entityVisuals.Values)
        {
            UnityEngine.Object.Destroy(visual);
        }
        entityVisuals.Clear();
    }

    private Dictionary<IEnvironmentObject, CellPosition> BuildOldPositionsMap()
    {
        var oldPositions = new Dictionary<IEnvironmentObject, CellPosition>();
        foreach (var pair in entityVisuals)
        {
            var entityVisual = pair.Value.GetComponent<EntityVisual>();
            if (entityVisual != null && entityVisual.EnvironmentObject != null)
            {
                oldPositions[entityVisual.EnvironmentObject] = pair.Key;
            }
        }
        return oldPositions;
    }

    private List<MovedObjectInfo> DetectMovedObjects(
        FieldState fieldState,
        Dictionary<IEnvironmentObject, CellPosition> oldPositions
    )
    {
        var movedObjects = new List<MovedObjectInfo>();

        foreach (var pair in fieldState.EnvironmentObjects)
        {
            var environmentObject = pair.Value;
            var newPosition = pair.Key;

            if (oldPositions.TryGetValue(environmentObject, out var oldPosition))
            {
                if (!oldPosition.Equals(newPosition))
                {
                    if (entityVisuals.TryGetValue(oldPosition, out var visual))
                    {
                        movedObjects.Add(
                            new MovedObjectInfo(
                                environmentObject,
                                visual,
                                oldPosition,
                                newPosition
                            )
                        );
                    }
                }
            }
        }

        return movedObjects;
    }

    private void AnimateMovedObjects(List<MovedObjectInfo> movedObjects)
    {
        foreach (var info in movedObjects)
        {
            DebugHelper.Log(
                DebugHelper.MessageType.Animation,
                $"Анимируем перемещение {info.EnvironmentObject.Name} с {info.OldPosition} на {info.NewPosition}"
            );

            Vector3 targetPos = getWorldPositionForObject(info.EnvironmentObject);
            var animation = new MoveAnimation(info.Visual.transform, targetPos, moveSpeed);
            animationQueue.EnqueueAnimation(animation);

            entityVisuals.Remove(info.OldPosition);
            entityVisuals[info.NewPosition] = info.Visual;
        }
    }

    private void CreateNewEntityVisuals(FieldState fieldState, Action<CellPosition> onCellClicked)
    {
        foreach (var pair in fieldState.EnvironmentObjects)
        {
            var position = pair.Key;
            var environmentObject = pair.Value;

            if (entityVisuals.ContainsKey(position))
                continue;

            GameObject prefab = GetPrefabForEntity(environmentObject);
            if (prefab == null)
                continue;

            string name = GenerateEntityName(environmentObject, position);
            Vector3 worldPos = getWorldPositionForObject(environmentObject);

            GameObject visual = UnityEngine.Object.Instantiate(
                prefab,
                worldPos,
                Quaternion.identity,
                parent
            );
            visual.name = name;

            EntityVisual entityVisual = visual.GetComponent<EntityVisual>();
            if (entityVisual == null)
                entityVisual = visual.AddComponent<EntityVisual>();

            entityVisual.Initialize(environmentObject, onCellClicked);
            entityVisuals[position] = visual;
        }
    }

    private GameObject GetPrefabForEntity(IEnvironmentObject environmentObject)
    {
        if (environmentObject is ICreature)
            return creatureVisualPrefab;
        if (environmentObject is Item)
            return itemVisualPrefab;
        return null;
    }

    private string GenerateEntityName(IEnvironmentObject environmentObject, CellPosition position)
    {
        string prefix = environmentObject is ICreature ? "Creature" : "Item";
        return $"{prefix}_{environmentObject.Name}_{position.X}_{position.Y}";
    }

    private void RemoveDestroyedEntityVisuals(FieldState fieldState)
    {
        var currentPositions = new HashSet<CellPosition>();
        foreach (var pair in fieldState.EnvironmentObjects)
        {
            currentPositions.Add(pair.Key);
        }

        var positionsToRemove = new List<CellPosition>();
        foreach (var pair in entityVisuals)
        {
            if (!currentPositions.Contains(pair.Key))
            {
                DebugHelper.Log(
                    DebugHelper.MessageType.Animation,
                    $"Удаляем визуал на позиции {pair.Key}"
                );
                UnityEngine.Object.Destroy(pair.Value);
                positionsToRemove.Add(pair.Key);
            }
        }

        foreach (var pos in positionsToRemove)
        {
            entityVisuals.Remove(pos);
        }
    }

    private readonly struct MovedObjectInfo
    {
        public readonly IEnvironmentObject EnvironmentObject;
        public readonly GameObject Visual;
        public readonly CellPosition OldPosition;
        public readonly CellPosition NewPosition;

        public MovedObjectInfo(
            IEnvironmentObject environmentObject,
            GameObject visual,
            CellPosition oldPosition,
            CellPosition newPosition
        )
        {
            EnvironmentObject = environmentObject;
            Visual = visual;
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }
}
