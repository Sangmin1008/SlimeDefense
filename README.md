# SlimeDefense

SlimeDefense는 Unity 기반의 클래식 타워 디펜스 게임입니다. 
그리드 위에 영웅을 배치하고, 적의 이동 경로와 웨이브 흐름을 방어하여 지휘관을 끝까지 지켜내는 것이 게임의 목표입니다.
단순한 기능 구현을 넘어, 유지보수성과 확장성, 그리고 극한의 최적화를 달성하기 위해 MVP 아키텍처, 의존성 주입(DI), 반응형 프로그래밍, 비동기 제어 등 현대적인 소프트웨어 설계 패턴과 고급 유니티 엔지니어링 기술을 엄격하게 적용하여 개발되었습니다.

---

## 주요 기술 스택 (Tech Stack)

* **Game Engine:** Unity 2D - 2022.3.62f3
* **Architecture:** MVP (Model-View-Presenter), Assembly Definition (Asmdef)
* **Dependency Injection:** VContainer
* **Reactive & Async:** UniRx, UniTask
* **Optimization:** Object Pooling, Sprite Atlas
* **Data Management:** ScriptableObject (Data-Driven Design)

---

## 핵심 아키텍처 및 최적화 (Core Architecture & Optimization)

본 프로젝트는 클래스 간의 결합도를 낮추고 데이터 흐름을 명확하게 제어하며, 앱 성능을 극대화하기 위해 아래의 설계 원칙을 따릅니다.

1. **MVP 분리:** 모든 주요 엔티티(Hero, Enemy, Commander, Projectile)는 상태와 데이터를 갖는 `Model`, 시각적 연출과 물리 충돌을 담당하는 `View`, 그리고 둘 사이의 비즈니스 로직을 연결하는 `Presenter`로 철저히 분리되어 있습니다.
2. **모듈화 및 단방향 의존성 (Asmdef):** 어셈블리 데피니션(Assembly Definition)을 적용하여 프로젝트를 `Core`(순수 데이터/로직), `Gameplay`, `Lobby`, `UI`, `Scopes`(DI 조립) 계층으로 완벽하게 분리했습니다. 이를 통해 스파게티 코드와 순환 참조(Circular Dependency)를 원천 차단하고, 스크립트 수정 시 컴파일 타임을 대폭 단축하여 개발 생산성을 확보했습니다.
3. **의존성 주입 (DI):** `InGameLifetimeScope` 및 최상위 `ProjectLifetimeScope`를 진입점으로 사용하여 Config, Manager, Presenter 등을 계층별로 바인딩하고 주입합니다. 이를 통해 싱글톤 패턴의 결합도 문제를 해결하고 완벽한 수명 주기 관리를 보장합니다.
4. **반응형 상태 동기화 (UniRx):** 체력 변동, 웨이브 진행 상태, 코인 증감 등의 핵심 데이터는 `UniRx`의 ReactiveProperty로 관리되며, UI 및 로직은 이를 구독하여 상태 변화에 즉각적으로 반응합니다.
5. **비동기 제어와 무가비지 애니메이션 (UniTask):** 웨이브 대기, 스폰 로직, 그리고 UI 팝업 연출(Tween) 등 시간에 의존하는 모든 비동기 작업을 코루틴 대신 `UniTask`로 구현하여 가비지 컬렉션(GC) 부하를 없앴습니다. 특히 UI 애니메이션은 `CancellationToken`을 활용해 오브젝트 파괴 시 즉시 안전하게 종료되도록 설계하여 메모리 누수와 에러를 완벽하게 차단했습니다.
6. **드로우 콜 최적화 (Sprite Atlas):** 수많은 2D 스프라이트와 UI 에셋들을 Sprite Atlas로 통합 패킹하여 단일 텍스처로 병합했습니다. 이로 인해 유니티의 배칭(Batching) 효율을 극대화하고 드로우 콜(Draw Call)을 획기적으로 줄여 모바일 디바이스에서도 쾌적한 렌더링 성능을 보장합니다.

---

## 기능별 세부 시스템 설명

### 1. 맵 및 그리드 상호작용 시스템 (Map & Grid System)
전장의 상태를 관리하고 플레이어의 배치 전략을 지원합니다.
* **GridClickDetector:** 타일맵 클릭을 감지하여 클릭한 셀 좌표와 월드 좌표를 반환합니다. 게임 시작 시 맵 전체를 스캔하여 파괴된 타일(Broken Cell) 데이터를 수집합니다.
* **GridManager:** 2D 그리드 데이터를 Dictionary와 HashSet으로 관리합니다. 셀의 점유 상태(IsEmpty), 영웅 배치 및 제거(PlaceHero/ClearCell), 파괴된 셀 관리를 전담하는 순수 데이터 관리자입니다.
* **GridInteractionPresenter:** 마우스 클릭 이벤트를 구독하여 셀의 상태에 따라 알맞은 행동을 처리합니다. 파괴된 칸은 수리(Repair), 빈 칸은 영웅 소환(Summon), 영웅이 존재하는 칸은 업그레이드(Upgrade) 팝업을 호출합니다.
* **StageInitializer:** 스테이지 시작 시 지휘관을 지정된 위치에 소환하고 Presenter를 연결합니다.
* **WaypointPath & PathDataSO:** 적의 이동 경로를 ScriptableObject 좌표 배열로 정의하여 다중 경로 확장이 가능하도록 설계되었습니다.

### 2. 엔티티 시스템 (Entity System)
아군 유닛, 적군, 방어 대상의 상태와 동작을 관리합니다.
* **Hero (아군 유닛):** `HeroManager`를 통해 코인을 소모하여 무작위 타입으로 소환됩니다. 동일한 등급(Grade)과 타입(Type)을 가진 영웅 두 마리를 병합하여 상위 등급(Legendary까지)으로 진화시킬 수 있습니다. 주변 적을 탐지하여 투사체 발사를 지시합니다.
* **Enemy (적군 유닛):** `EnemyRegistry`를 통한 레지스트리 패턴으로 활성화된 적을 추적합니다. 경로를 따라 이동하며, 처치될 경우 코인을 드롭하고 목적지 도달 시 지휘관에게 데미지를 입힙니다.
* **Commander (지휘관):** 게임의 핵심 방어 대상입니다. 적 탈출 이벤트 발생 시 체력이 감소하며, 체력이 0이 되면 게임 오버를 트리거합니다.

### 3. 웨이브 시스템 (Wave System)
게임의 흐름과 난이도를 제어합니다.
* **WaveModel:** 현재 웨이브 인덱스, 생존한 적 수, 다음 웨이브까지의 대기 시간, 게임 오버 여부를 반응형으로 관리합니다.
* **EnemySpawner:** `UniTask` 기반의 루프를 사용하여 웨이브 대기 시간 카운트다운과 적 스폰 로직을 비동기로 처리합니다. 게임 오버 시 CancellationToken을 활용해 스폰을 즉각 중단합니다.
* **WavePresenter:** 지휘관 사망 시 패배 처리, 마지막 웨이브의 모든 적 섬멸 시 승리 처리를 담당합니다.

### 4. 전투 및 투사체 시스템 (Projectile System)
물리 연산 최적화를 담당합니다.
* **ProjectileManager:** 프리팹별로 `ObjectPool<ProjectileView>`를 유지하여 발사체의 동적 생성/파괴로 인한 성능 저하를 방지합니다.
* **유도 및 관통 로직:** 영웅이 조준한 타겟을 향해 이동하며, 이동 도중 타겟이 사망하더라도 마지막 방향으로 직진하여 다른 적에게 데미지를 입힐 수 있도록 설계되었습니다.
* **EnemyRegistry 연동:** 투사체가 EnemyView와 충돌하면 Registry를 통해 매핑된 실제 EnemyModel을 O(1) 탐색으로 찾아내 데미지를 적용합니다.

### 5. 경제 시스템 (Coin System)
전투와 성장을 연결하는 선순환 자원 루프입니다.
* **CoinModel:** 현재 보유 코인을 ReactiveProperty로 관리합니다. 초기 자본은 StageConfig에서 설정됩니다.
* 적 처치 시 Config에 정의된 보상만큼 즉시 코인이 증가하며, 소환/승급/수리 시 일관된 비용 차감 로직(`TrySpendCoin`)을 거칩니다. 영웅 등급별 비용은 `HeroCostHelper`를 통해 동적으로 계산됩니다.

### 6. UI 시스템 (Shared UI & State Synchronization)
* **UI 어셈블리:** 비즈니스 로직과 철저히 분리된 `SlimeDefense.UI` 모듈을 구축하여 팝업, 버튼 클릭 연출(`ProgressTweener`) 등을 UniTask 기반으로 부드럽게 렌더링합니다.
* **GameUIView:** 커맨더 체력, 웨이브 정보, 코인, 그리드 액션 팝업, 결과 화면 등 시각적 렌더링만을 담당합니다.
* **GameUIPresenter:** 게임 내 다양한 Model(Wave, Commander, Coin)의 상태를 UniRx로 구독하여 View의 데이터를 갱신합니다. 씬 전환 시 에러가 발생하지 않도록 철저한 Dispose 처리를 포함합니다.

### 7. 데이터 주도 설계 (Data & Configuration)
모든 밸런스 수치와 기획 데이터는 프로그래머의 스크립트 개입 없이 유니티 에디터 상에서 수정 가능하도록 설계되었습니다.
* **StageConfig:** 초기 코인, 커맨더 설정 및 위치, 포함할 웨이브 목록 관리.
* **WaveConfig:** 웨이브별 적 스폰 종류, 수량, 간격, 대기 시간 정의.
* **HeroConfig / EnemyConfig / CommanderConfig:** 체력, 공격력, 사거리, 이동 속도, 투사체 매핑, 보상 등의 개별 스탯 관리.

---

## 플레이 흐름 요약 (Game Flow)

1. **초기화:** 게임 실행 시 `ProjectLifetimeScope`가 전역 데이터를 세팅하고, 인게임 진입 시 `InGameLifetimeScope`가 의존성을 주입하여 `StageInitializer`가 지휘관을 배치합니다.
2. **배치 및 수리:** 플레이어는 초기 코인을 활용해 깨진 타일을 복구하거나 빈 그리드에 영웅을 소환합니다.
3. **전투 진행:** 지정된 대기 시간 후 적 웨이브가 경로를 따라 생성됩니다. 영웅들은 자동 타겟팅으로 적을 처치합니다.
4. **성장:** 적 처치로 획득한 코인을 사용해 영웅을 추가 소환하고, 동일 등급/타입의 영웅을 병합하여 상위 등급으로 업그레이드합니다.
5. **결과:** 구성된 모든 웨이브의 적을 막아내면 승리하며, 방어선이 무너져 지휘관의 체력이 0이 되면 패배 화면으로 전환됩니다.
