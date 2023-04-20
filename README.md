# ProjetoN1-IA

PROJETO N1 - INTELIGENCIA ARTIFICIAL

- RESUMO -
O projeto consiste em uma simulação por turno onde existe uma caça e de 5 a 10 presas. Ao iniciar o programa a simulação define locais aleatórios para o caçador e em seguida define um número aleatório de 5 a 10 e posiciona cada uma delas em uma posição não ainda ocupada. Ao pressionar espaço o jogador começa/pausa o "autoplay", modo no qual todas as entidades fazem seu movimento em turn automáticamente (primeiro o hunter e depois as presas). Ao invés de espaço pode se controlar a simulação apertando T para avançar apenas um turno da simulação. Quando a ultima presa morre, a simulação acaba.

- COMO FOI FEITO -

O projeto foi feito em C# na Unity e é formado, basicamente por 7 objetos, são eles: o Caçador, a Presa, a Entidade, o GridManager, o GridObject, o GameManager e o HudManager. A seguir vou explicar como cada um deles se comporta e interage com as outras classes.

- ENTIDADE -
  - A classe Entity é uma classe base que é herdada pelo caçador e pela presa, nela se define as funções e variáveis comuns às duas entidades do jogo como, por exemplo, CheckAvailableMoves() que checa as posições de grid ao redor do objeto e determina quais posições são válidas para este ocupar e MoveRandom() que move ele aleatóriamente para uma posição já checada presente na sua lista (também definida nesta classe base) availableMoves.
  
- CAÇADOR -
  - A classe caçador herda da classe base Entity e implementa algumas funções específicas do caçador, são elas:
      - private void CheckForPrey()
        - Checa dentro do alcance de visão do Hunter se tem alguma presa e retorna ela. Se tiver mais de uma presa ou se a presa estiver mais próxima que o alvo atual retorna a presa mais próxima.
        
      - public Prey GetCurrentTarget()
        - Retorna a presa atual que o caçador esta perseguindo.
      
      - private GridObject GetClosestMoveToTarget(Prey target)
        - Calcula dentro da lista de movimentos disponíveis o movimento que deixará o caçador mais próximo da presa.
      
      - private void MoveInTargetDirection(Prey target)
        - Move o caçador em direção a presa utilizando o método GetClosestMoveToTarget (Prey target)
      
      - private bool IsInAttackRange()
        - Checa se o caçador está em alcance para atacar a presa.
      
      - private void GameManager_OnHunterTurn(object sender, EventArgs e)
        - Contém a máquina de estados do caçador e é chamada a partir de um evento (OnHunterTurn) disparado da classe GameManager. No Start() o caçador começa a escutar este evento e realiza as ações determinadas em sua máquina de estados de acordo com o estado atual sempre que esse evento é disparado no GameManager. Esta implementação utiliza o padrão do Observador (Singleton).

- PRESA -
  - A classe presa também herda da classe base Entity e tem algumas funções específicas, são elas:
    - private void GameManager_OnPreyTurn(object sender, EventArgs e)
      - Assim como o Caçador este método escuta o evento OnPreyTurn disparado da classe GameManager e quando escuta esse evento, performa as ações determinadas no seu estado atual.
    
    - public void Die()
      - Método que destrói o GameObject da presa que foi caçada.
      
    - private bool IsBeingChased() 
      - Checa se o currentTarget do Caçador é igual ao próprio GameObject para saber se está sendo caçado e deve fugir do caçador.
      
    - private void MoveToFurthestFromHunter() 
      - Checa os movimentos disponíveis, escolhe o local mais longe do caçador dentre estes e se move para ele.
      
    - private void OnDestroy() 
      - Método que é chamado quando o GameObject é destruído, necessário para cleanup já que está sendo utilizado a Observer Pattern.
    
- GRIDOBJECT -
  - Script vazio utilizado apenas para detectar os GridObjects e referencia-los em código.

- GRIDMANAGER -
  - A classe GridManager é responsável por manuseiar e criar os grids no inicio do jogo. Tem duas funções:
    - private void Start()
      Aqui ela cicla pelo número de colunas e linhas e cria para cada posição um GridObject e no fim da criação, chama um evento para que o GameManager saiba quando criar o Caçador e as Presas, já que estes dependem de ter uma Grid criada para se posicionar.
      
    - private void GetWorldPosition(int x, int z)
      - Método utilizado para definir a posição no mundo de cada GridObject em sua criação levando em consideração o número de células que foi criado até agora e o tamanho de cada.
- GAMEMANAGER -
  Controla vários aspectos do jogo como quem vai jogar, numero de turnos, ordem de spawn, etc. Sua funções são:
    - private void GridManager_OnGridSpawned(object sender, EventArgs e)
      - Chama as funções que só podem ser chamadas após a grid ter sido criada: PrepareSpawn(), SpawnHunter() e SpawnPrey().
      
    - private void Hunter_OnPreyKilled(object sender, EventArgs e) 
      - Diminui a preyCount deste script para atualizar quantas Presas tem vivas na interface.
    
    - private void Hunter_OnHunterPlayed(object sender, EventArgs e) 
      - Escuta o evento disparado pelo Hunter ao terminar de jogar, para depois disparar o evento para a presa jogar.
    
    - private void Update()
      - Dentro deste método estão as checagens para ligar o autoplay com Espaço ou avançar um turno com a letra T.
      
    - private void PrepareSpawn()
      - Adiciona todos os GridObjects na lista de locais disponíveis para spawn.
      
    - private void SpawnHunter()
      - Cria o objeto do hunter e exclui um raio de 5 casas envolta dele para nenhuma presa conseguir spawnar e já nascer sendo perseguida.
      
    - private void SpawnPrey()
      - Cria um número aleatório de 5-10 presas e dispõe eles nas posições disponíveis de forma que sempre que posiciona um, exclui as casas em um raio de 3 quadrados ao redor para que nenhuma presa spawne muito próxima de outra. 
    
- HUDMANAGER -
  - É uma classe simples que tem a função de atualizar informações relevantes na interface do jogo. Tem apenas o método Update() onde atualiza o valor de turnCount e remainingEnemies do GameManager na interface do programa.
