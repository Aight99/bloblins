// using System.Collections.Generic;
// using NUnit.Framework;

// // Пример тестового класса для GameReducer
// public class GameReducerTests
// {
//     [Test]
//     public void LoadLevel_CreatesCorrectState()
//     {
//         // Arrange
//         var initialFieldState = new FieldState(0, 0, new Dictionary<CellPosition, IEntity>());
//         var initialState = new GameState(initialFieldState, 0);

//         // Act
//         var action = new LoadLevelAction(1);
//         var newState = GameReducer.Reduce(initialState, action);

//         // Assert
//         Assert.AreEqual(1, newState.CurrentLevel);
//         Assert.AreEqual(10, newState.Field.Width);
//         Assert.AreEqual(10, newState.Field.Height);
//         Assert.IsTrue(newState.Field.Entities.Count > 0);
//     }

//     [Test]
//     public void SelectBloblin_UpdatesSelectedBloblin()
//     {
//         // Arrange
//         var bloblin = new Bloblin("Test") { X = 1, Y = 1 };
//         var position = new CellPosition(1, 1);
//         var entities = new Dictionary<CellPosition, IEntity> { { position, bloblin } };
//         var fieldState = new FieldState(10, 10, entities);
//         var state = new GameState(fieldState, 1);

//         // Act
//         var action = new SelectBloblinAction(bloblin);
//         var newState = GameReducer.Reduce(state, action);

//         // Assert
//         Assert.AreEqual(bloblin, newState.Field.SelectedBloblin);
//     }

//     [Test]
//     public void CellClick_OnEmptyCell_WithSelectedBloblin_MovesBloblin()
//     {
//         // Arrange
//         var bloblin = new Bloblin("Test") { X = 1, Y = 1 };
//         var startPosition = new CellPosition(1, 1);
//         var targetPosition = new CellPosition(2, 2);
//         var entities = new Dictionary<CellPosition, IEntity> { { startPosition, bloblin } };
//         var fieldState = new FieldState(10, 10, entities, bloblin);
//         var state = new GameState(fieldState, 1);

//         // Act
//         var action = new CellClickAction(targetPosition);
//         var newState = GameReducer.Reduce(state, action);

//         // Assert
//         Assert.IsFalse(newState.Field.Entities.ContainsKey(startPosition));
//         Assert.IsTrue(newState.Field.Entities.ContainsKey(targetPosition));
//         Assert.AreEqual(bloblin, newState.Field.Entities[targetPosition]);
//         Assert.AreEqual(2, bloblin.X);
//         Assert.AreEqual(2, bloblin.Y);
//     }

//     [Test]
//     public void CellClick_OnOccupiedCell_DoesNotMove()
//     {
//         // Arrange
//         var bloblin1 = new Bloblin("Test1") { X = 1, Y = 1 };
//         var bloblin2 = new Bloblin("Test2") { X = 2, Y = 2 };
//         var position1 = new CellPosition(1, 1);
//         var position2 = new CellPosition(2, 2);
//         var entities = new Dictionary<CellPosition, IEntity>
//         {
//             { position1, bloblin1 },
//             { position2, bloblin2 }
//         };
//         var fieldState = new FieldState(10, 10, entities, bloblin1);
//         var state = new GameState(fieldState, 1);

//         // Act
//         var action = new CellClickAction(position2);
//         var newState = GameReducer.Reduce(state, action);

//         // Assert
//         Assert.IsTrue(newState.Field.Entities.ContainsKey(position1));
//         Assert.IsTrue(newState.Field.Entities.ContainsKey(position2));
//         Assert.AreEqual(bloblin1, newState.Field.Entities[position1]);
//         Assert.AreEqual(bloblin2, newState.Field.Entities[position2]);
//         Assert.AreEqual(1, bloblin1.X);
//         Assert.AreEqual(1, bloblin1.Y);
//     }
// }
