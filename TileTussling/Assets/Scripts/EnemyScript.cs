using System.Collections;
using System.Collections.Generic;
using MapStuff;
using UnityEngine;

public class EnemyScript : TileItemScript
{
    public enum MoveAction {up, down, left, right, attackUp, attackDown, attackLeft, attackRight};
    [SerializeField] int obstacleInPathPenalty;
    private struct path
    {
        public List<MoveAction> actionList;
        public int actionCost;
        public TileScript finalPosition;

        public static path startNewPath()
        {
            path obj = new path();
            obj.actionList = new List<MoveAction>();
            obj.actionCost = 0;
            return obj;
        }

        public static path branchFromPath(path previous)
        {
            path obj = new path();
            obj.actionList = new List<MoveAction>(previous.actionList);
            obj.actionCost = previous.actionCost;
            return obj;
        }
    }

    private List<path> paths = new List<path>();

    private void CalculateCostToEachTile()
    {
        // trick is to start from your tile, and get every direction from there. After assigning those tiles a path, you make new paths continuing from the old ones.
        //If the paths lead up to the same spaces as a previous path, compare costs. If one of them is higher, delete it from the list.
        //Should theoretically end once all tiles have been assigned a cost to get to them

        var startingTile = transform.parent.gameObject.GetComponent<TileScript>();
        var pathStart = path.startNewPath();
        pathStart.finalPosition = startingTile;
        paths.Add(pathStart);


    }

    private void CalculateAdjacentPathings(path originalPath)
    {
        var topTile = originalPath.finalPosition.topTile;
        var leftTile = originalPath.finalPosition.leftTile;
        var bottomTile = originalPath.finalPosition.bottomTile;
        var rightTile = originalPath.finalPosition.rightTile;

        BranchPathToTile(originalPath, topTile, MoveAction.up);
        BranchPathToTile(originalPath, leftTile, MoveAction.left);
        BranchPathToTile(originalPath, bottomTile, MoveAction.down);
        BranchPathToTile(originalPath, rightTile, MoveAction.right);
    }

    private void BranchPathToTile(path originalPath, TileScript newTile, MoveAction direction)
    {
        if(TileIsValidPath(newTile))
        {
            var newPath = path.branchFromPath(originalPath);
            newPath.finalPosition = newTile;
            newPath.actionList.Add(direction);
            newPath.actionCost++;

            if(newTile.heldItem != null)
                newPath.actionCost += obstacleInPathPenalty;
            
            for(int i = 0; i < paths.Count; i++)
            {
                
            }
        }
    }

    private bool TileIsValidPath(TileScript tile)
    {
        return (tile.tileType != TileType.empty);
    }

    private void CalculatePathToPlayer()
    {
        // First get distance between the player
        // Maybe use A* algorithm?
        var distanceToCross = PlayerScript.player.transform.position - transform.position;

    }
}
