using System;

namespace GameLogic
{
	public enum NotificationEnum
	{
		//Town
		TOWN_HERO_INIT,
		TOWN_NPC_SPAWN,
		TOWN_HERO_DISPOSE,

		//Maze
		INPUT_INIT,
		INPUT_ENABLE,

		BLOCK_INIT,
		BLOCK_DISPOSE,
        BLOCK_REFRESH,
        BLOCK_SHOW_ALL,
		BLOCK_HIDE_ALL,
        BLOCK_SPAWN,
        BLOCK_DESPAWN,

		HALL_INIT,
		HALL_DISPOSE,
        HALL_SPAWN,
        HALL_DESPAWN,

		MAZE_MAP_SHOW,
		MAZE_MAP_RESET,
		
		HERO_INIT,
        HERO_CONVERT_START,
		HERO_CONVERT_END,
		HERO_TRANSPORT,

        MONSTER_INIT,
        MONSTER_DISPOSE,

		BULLET_SPAWN,
		BULLET_DESPAWN,

		DROP_CREATED,
		DROP_PICKED_UP,

		ENVIRONMENT_INIT,
        ENVIRONMENT_DAYNIGHT_CHANGE,
        ENVIRONMENT_SHOW_MAZE_MAP,

		NPC_INIT,
		NPC_DISPOSE,
		NPC_DIALOG_SHOW,

		BATTLE_UI_INIT,
		BATTLE_UI_UPDATE_HP,

		BATTLE_PAUSE,

        //Record
        DESERIALIZE_GAME,
        SERIALIZE_GAME,

        //Input
        MOUSE_HIT_OBJECT,
        KEY_DOWN,
        DRAG_START,
        DRAG_END,

        #region Pack
        PACK_SHOW,
        PACK_REFRESH,
        PACK_CHANGE_ITEM_COUNT,
        PACK_USE_ITEM,
        #endregion

        #region Profile
        PROFILE_SHOW,
        #endregion

        #region Pathfinding
        PATHFINDING_INIT,
        PATHFINDING_DISPOSE,
        #endregion
	}
}
