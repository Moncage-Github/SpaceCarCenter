# Prototype

Enemy Ai System

기본 에너미는 State 패턴을 사용해 구현하였으며 None, Idle, Move, Attack, Skill, Dead 상태를 enum class로 가진다. 
해당 상태들은 모두 Class로 구현되었으며 IEnemyState 인터페이스를 상속받는다.
에너미의 상태에 따라 _enemyState(IEnemyState)에 해당 상태를 넣어주고 IEnemyState를 통해 구현된 Update함수를 실행한다.


MiniMap System

카메라를 하나 더 많든 뒤 Layout에 Minimap 레이아웃을 추가해 해당 오브젝트만 보이게 설정한다.


UpgradeUI
