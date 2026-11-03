using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public static class UtilsClass {
    public struct GridDimensions {
        public int columnCount;
        public int rowCount;

        public GridDimensions(int columnCount, int rowCount) {
            this.columnCount = columnCount;
            this.rowCount = rowCount;
        }

        public override readonly string ToString() {
            return $"columnCount: {columnCount}, rowCount: {rowCount}";
        }
    }

    public static Vector3 GetMouseWorldPosition() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPosition.z = 0;

        return mouseWorldPosition;
    }

    public static Vector3 GetRandomDir() {
        return new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f)
        ).normalized;
    }

    public static Vector2 GetGridLayoutGroupContentSize(GridLayoutGroup gridLayoutGroup) {
        int childCount = GetGridLayoutGroupActiveChildCount(gridLayoutGroup);
        if (childCount <= 0) {
            return Vector2.zero;
        }

        Vector2 cellSize = gridLayoutGroup.cellSize;
        Vector2 spacingSize = gridLayoutGroup.spacing;
        GridDimensions gridDimensions = GetGridLayoutGroupGridDimensions(gridLayoutGroup);

        float contentWidth = (cellSize.x * gridDimensions.columnCount) + (spacingSize.x * (gridDimensions.columnCount - 1));
        float contentHeight = (cellSize.y * gridDimensions.rowCount) + (spacingSize.y * (gridDimensions.rowCount - 1));

        return new Vector2(contentWidth, contentHeight);
    }

    public static GridDimensions GetGridLayoutGroupGridDimensions(GridLayoutGroup gridLayoutGroup) {
        int childCount = GetGridLayoutGroupActiveChildCount(gridLayoutGroup);
        if (childCount <= 0) {
            return new GridDimensions(0, 0);
        }

        int constraintCount = gridLayoutGroup.constraintCount;
        int columnCount;
        int rowCount;

        switch (gridLayoutGroup.constraint) {
            case GridLayoutGroup.Constraint.FixedColumnCount:
                columnCount = childCount < constraintCount ? childCount : constraintCount;
                rowCount = GetRequiredAxisCount(childCount, columnCount);

                break;
            case GridLayoutGroup.Constraint.FixedRowCount:
                rowCount = childCount < constraintCount ? childCount : constraintCount;
                columnCount = GetRequiredAxisCount(childCount, rowCount);

                break;
            case GridLayoutGroup.Constraint.Flexible:
                Vector2 rectTransformSize = gridLayoutGroup.GetComponent<RectTransform>().rect.size;
                Vector2 paddingSize = new Vector2(gridLayoutGroup.padding.horizontal, gridLayoutGroup.padding.vertical);
                Vector2 availableSize = rectTransformSize - paddingSize;
                Vector2 cellSize = gridLayoutGroup.cellSize;
                Vector2 spacingSize = gridLayoutGroup.spacing;

                switch (gridLayoutGroup.startAxis) {
                    case GridLayoutGroup.Axis.Horizontal:
                        if (availableSize.x < cellSize.x * 2) {
                            columnCount = 1;
                            rowCount = childCount;

                            break;
                        }

                        columnCount = GetMaxCellsPerAxis(availableSize.x, cellSize.x, spacingSize.x);
                        columnCount = childCount < columnCount ? childCount : columnCount;
                        rowCount = GetRequiredAxisCount(childCount, columnCount);

                        break;
                    case GridLayoutGroup.Axis.Vertical:
                        if (availableSize.y < cellSize.y * 2) {
                            rowCount = 1;
                            columnCount = childCount;

                            break;
                        }

                        rowCount = GetMaxCellsPerAxis(availableSize.y, cellSize.y, spacingSize.y);
                        rowCount = childCount < rowCount ? childCount : rowCount;
                        columnCount = GetRequiredAxisCount(childCount, rowCount);

                        break;
                    default:
                        throw new NotImplementedException($"StartAxis {gridLayoutGroup.startAxis} not implemented!");
                }

                break;
            default:
                throw new NotImplementedException($"Constraint {gridLayoutGroup.constraint} not implemented!");
        }

        return new GridDimensions(columnCount, rowCount);
    }

    public static int GetGridLayoutGroupActiveChildCount(GridLayoutGroup gridLayoutGroup) {
        int activeChildCount = 0;

        foreach (Transform child in gridLayoutGroup.transform) {
            if (child.gameObject.activeSelf) {
                activeChildCount++;
            }
        }

        return activeChildCount;
    }

    public static int GetMaxCellsPerAxis(float axisAvailableSize, float contentAxisSize, float spacingAxisSize) {
        return Mathf.FloorToInt((axisAvailableSize + spacingAxisSize) / (contentAxisSize + spacingAxisSize));
    }

    public static int GetRequiredAxisCount(int childCount, float axisCount) {
        return Mathf.CeilToInt((float)childCount / axisCount);
    }
}

