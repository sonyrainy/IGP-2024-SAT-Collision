**인디게임제작(Indie Game Production)_Tutorial 제작 프로젝트 입니다.**

>**SAT와 AABB 이론을 활용해 충돌처리를 자체적으로 구현하였습니다.**

+) 01_Scenes, 02_Scripts, 03_Prefabs 폴더만 업로드하였습니다.

---

### 1. 프로젝트(게임) 특징
>- **기본 조작**: wasd를 이용한 이동 및 점프, spacebar를 통한 공격
>- **클릭을 이용한 영역(TimeZone) 생성**: 마우스 클릭으로 화면 상 영역을 생성하여 그 내부에 들어오는 오브젝트의 시간이 빠르게 흐르도록(속도 증가) 설정


→ **다각형의 충돌 가능성 검사(AABB) 후, 충돌 정밀 검사(SAT)를 진행하는 방식으로 충돌 로직을 구현하였습니다.**

<br>

<div align="center">
    <img src="https://github.com/user-attachments/assets/307b1f6f-ff42-48be-9dd0-0578f2ae3736" alt="image" width="320" height="145">
</div>

<br>


### 2. AABB(Axis – Aligned Bounding Box), SAT(Seperating Axis Theorem)
#### - AABB란?
>- 두 물체(직사각형))의 x, y 축에으로 투영되는 최대, 최소 거리 값을 계산 → 두 물체 간의 축 범위가 겹치는지 검사 → 두 축(x축, y축)에서 모두 겹치는 것이 확인되면 충돌으로 판단한다.
>- 사각형이 아닌 다른 다각형 오브젝트의 경우, 각각의 vertex가 x축으로 가는 x값의 최댓값, y축으로 가는 y값의 최댓값을 구해서 오브젝트를 감싸는 임의의 사각형을 생성 후,
그 두 사각형을 비교하여 AABB를 검사한다.

<br>


<div align="center">
    <img src="https://github.com/user-attachments/assets/a9fe2347-d6eb-4946-878c-7ae64f99d39d" alt="image" width="400" height="300">
</div>

<br>

#### - SAT란?
>- SAT는 두 개의 볼록 다각형이 충돌하지 않으면, 두 다각형을 완전히 분리하는 축이 존재하는 것을 원리로 작동한다.
>- 만약 모든 가능한 축에서 두 다각형의 꼭짓점이 “내적” 즉, “투영”된(도형이 빛을 통해 투영된 것)이 겹치면 충돌이 발생한 것으로 판단한다.
>- 각 다각형의 변에 대해 법선 벡터를 계산하고, 그 벡터를 축으로 사용한다. 두 다각형의 모든 변에 대해 이러한 분리 축을 생성한 후, 그 축에 두 다각형을 투영(내적)한다. 내적된 부분이 겹치지 않으면 두 다각형은 충돌하지 않는 것으로 간주된다.
>- 모든 축에서 투영(내적)이 겹치는 부분이 있다면, 두 다각형은 충돌한 것으로 판단한다.

<br>

<div align="center">
    <img src="https://github.com/user-attachments/assets/acc8956e-4fae-4486-a204-233aebbe9ae0" alt="image" width="550" height="270">
</div>

<br>

### 3. 스크립트별 역할

- **GameManager.cs**: TimeZone을 생성한다. 게임 과정 중, 충돌 처리 여부를 CollisionManager를 통해 진행되도록 한다.
- **CollisionManager.cs**: 충돌할 수 있는 오브젝트를 관리하고, AABB(Axis-Aligned Bounding Box)와 SAT(Separating Axis Theorem)를 이용해 충돌 여부를 판단한다. AABB로 충돌 가능성을 판단 후, SAT 알고리즘으로 정밀한 충돌 여부를 검사(계산)한다.
- **SATCollisionObject.cs**: 물체의 PolygonCollider2D에서 꼭짓점을 가져와 충돌 연산에 필요한 오브젝트의 정보를 CollisionManager에 넘겨준다.
- **Bullet.cs**: 총알의 이동과 속도 관리 및 총알이 TimeZone과 충돌했을 때, 충돌을 빠져나갔을 때의 로직을 담당한다.
- **PlayerController.cs**: 플레이어의 이동, 점프, 총알 발사 등의 기본적인 제어를 담당합니다. 플레이어가 TimeZone 안에 있을 때의 로직을 담당한다.
- **Enemy.cs**: 적의 체력 관리 및 데미지를 입거나, 죽는 로직을 담당한다.

---


+) 참고: [AABB](https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection), [SAT_1](https://www.sevenson.com.au/programming/sat/), [SAT_2](https://programmerart.weebly.com/separating-axis-theorem.html), [SAT_3](https://www.youtube.com/watch?v=dn0hUgsok9M)

