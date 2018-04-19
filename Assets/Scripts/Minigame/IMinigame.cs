using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 미니 게임 종료 조건:
// 1) 게임 시간이 다 됨
// 2) Tick()이 true를 반환함
public interface IMinigame {
	void Setup(); // 게임 초기화

	void Finished(); // 게임이 종료되면 불림

	float Progress { get; } // 포인트의 진척 상황, [0,1] 값이어야 한다

	float MaxTime { get; } // 게임 시간

	bool Tick(); // 매 프레임마다 호출됨. true를 반환하면 게임 종료
}
