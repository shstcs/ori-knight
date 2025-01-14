# 간단한 2D 매트로베니아 게임
## Develop Goal 
* 감 찾기, Unity 6 살펴보기.
## Develop Schedule 
* 25.01.07 ~ 25.01.14
## Game Screenshot
![image](https://github.com/user-attachments/assets/488de2c1-3710-4dc2-8949-afba2d6ec934)
![image](https://github.com/user-attachments/assets/db58bc2e-626d-411a-9447-02b859f3ed2d)
## 세부 구현 기능
### 장르 기본 문법
기본적인 Attack, Dash, Jump 의 조작과, 2단점프, 벽타기, 체력 회복, 스킬 획득, 쿨타임 등 장르의 기본적인 문법 구현.
### Deadzone Camera
- 플레이어가 중앙의 일정 Zone을 벗어나게 되면 카메라가 따라가는 식으로 구현하였음.
### AudioSourcePool
- Attack/Jump/Dash 등 여러 사운드를 내기 위해 AudioManager에서 Object Pooling을 생성.
### Particle
Heal, Move, Dash 등 여러 입력마다 각각의 Particle을 제작해보며 사용법을 익힘.
