# 3D Unity Project

이 프로젝트는 Unity 3D 환경에서 플레이어의 생존 상태(체력, 허기, 스태미나)를 관리하며, 
이를 바탕으로 한 간단한 생존형 게임을 구현하는 것을 목표로 합니다.
상태 수치는 UI와 연동되며, 특정 조건에서 체력이 감소하거나 플레이어가 사망합니다.

---

## 주요 기능

- `Condition` 클래스: 각 상태의 현재값, 최대값, 자연 회복/소모 속도 관리
- `UICondition`: 3가지 상태를 UI 바에 실시간 반영
- `PlayerCondition`: 매 프레임마다 hunger, stamina 상태 갱신 및 death 조건 처리
- `CharacterManager`: 싱글톤으로 Player에 전역 접근 제공
- `PlayerController`: 마우스 회전, 이동, 점프 등 기본 조작 구현

---

##  트러블슈팅 요약

### ‧₊˚ 1. Float 비교로 인한 체력 감소 실패 ‧₊˚

  ੈ ✩  문제 (Problem)  
  플레이어의 `hunger`가 0이 되어도 `health`가 줄지 않고 `Die()`도 호출되지 않음

  ੈ ✩  시도한 점 (What was tried)  
  `== 0f` 비교 사용, 로그 찍기, `Subtract()` 호출 확인

  ੈ ✩  해결 (Solution)  
  float은 소수점 오차로 인해 `==` 비교가 실패할 수 있음 → `<= 0f`로 변경하여 조건 통과

  ੈ ✩  알게 된 점 (What was learned)  
  **float 값 비교 시 항상 `<=`, `>=` 같은 비교 연산자를 사용하는 것이 안전**  
  정밀도가 요구되는 상황에서는 `==`은 매우 위험함

---

### ‧₊˚ 2. 싱글톤 Instance 접근 오류 ‧₊˚

  ੈ ✩  문제 (Problem)  
  `Player.cs`에서 `CharacterManager.Instance.Player = this;` 호출 시  
  **`Instance`가 정의되지 않았다는 컴파일 오류 발생**

  ੈ ✩  시도한 점 (What was tried)  
  `CharacterManager` 내부 확인 결과  
  `public static CharacterManager InstanceManager`로 되어 있어 이름이 달랐음

  ੈ ✩  해결 (Solution)  
  싱글톤 속성명을 `InstanceManager` → `Instance`로 변경하여  
  외부에서 `CharacterManager.Instance`로 통일된 접근 가능하게 수정

  ੈ ✩  알게 된 점 (What was learned)  
  **싱글톤 패턴에서 Instance 명칭은 통일이 매우 중요**  
  **이름 하나 다르게 썼다고 전체 시스템이 작동하지 않을 수 있음** → 네이밍 규칙의 중요성 체감

---

##  사용 방법

1. **씬에 Player 프리팹 배치**
   - `Player.cs`, `PlayerController.cs`, `PlayerCondition.cs`가 연결되어 있어야 합니다.
   
2. **UICondition 오브젝트 배치**
   - `health`, `hunger`, `stamina`에 각각 `Condition` 스크립트와 UI Image를 연결합니다.

3. **CharacterManager는 자동 생성됨**
   - 별도로 배치할 필요 없음. `Player`가 실행 시 자동 등록합니다.

4. **Input System 사용**
   - `PlayerInput` 컴포넌트와 Input Action Asset이 연결되어 있어야 키 입력이 동작합니다.
   - 이동(WSAD), 회전(마우스), 점프(Space) 가능

5. **아이템/점프대/조사 기능 사용 시**
   - 아이템은 `ScriptableObject`로 작성 후 인스펙터에 등록
   - 점프대에는 `Collider + OnCollisionEnter` 처리 스크립트 부착
   - 조사 대상 오브젝트는 `Raycast`로 감지될 수 있는 위치에 배치 필요

---

### 추가 구현 예정

- **동적 환경 조사** (`Raycast`, `UI`, ★★★☆☆)  
  플레이어가 바라보는 오브젝트를 `Raycast`로 감지하고, 해당 오브젝트의 이름/설명을 UI에 표시

- **점프대 시스템** (`Rigidbody`, `ForceMode.Impulse`, ★★★☆☆)  
  캐릭터가 특정 점프대에 닿을 경우, `OnCollisionEnter` 이벤트를 활용하여 순간적으로 위로 튀어오르게 구현

- **아이템 데이터 관리** (`ScriptableObject`, ★★★☆☆)  
  아이템의 이름, 설명, 효과 등의 데이터를 `ScriptableObject`로 분리하여 재사용성과 유지 보수성 향상

- **아이템 사용 시스템** (`Coroutine`, ★★★☆☆)  
  일정 시간 동안 지속되는 아이템 효과 구현  
  예: 일정 시간 스피드 부스트 효과 → `Coroutine`을 활용한 일시적 상태 변경

---

## 개선 여지

- 상태 수치 저장 및 로드 기능 추가 (세이브/로드 시스템)
- 플레이어 사망 후 리스폰 또는 게임 오버 연출 구현
- 상태 이상(중독, 피로 등)이나 시간 기반 디버프 시스템 확장 가능성
- 조사 UI 개선 (아이템, NPC 등 특정 오브젝트별 고유 정보 출력)
- 점프대 물리감 조절 및 다양한 ForceMode 실험
- ScriptableObject를 활용한 **퀘스트, 적 캐릭터, 환경 정보 등 데이터 확장 구조화**
- 지속 효과 아이템의 종류와 조건 분기(예: 중첩 제한, 효과 우선순위) 세분화
