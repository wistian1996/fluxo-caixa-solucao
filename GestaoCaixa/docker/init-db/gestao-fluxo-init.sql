CREATE DATABASE gestao_fluxo;

CREATE USER 'gestao_fluxo_user_api'@'%' IDENTIFIED BY '123456';

GRANT ALL ON gestao_fluxo.* TO 'gestao_fluxo_user_api'@'%';

-- saldo_consolidado

CREATE DATABASE saldo_consolidado;

CREATE USER 'saldo_consolidado_user_api'@'%' IDENTIFIED BY '123456';

GRANT ALL ON saldo_consolidado.* TO 'saldo_consolidado_user_api'@'%';

USE saldo_consolidado;

CREATE TABLE saldo_consolidado_diario
(
    comerciante_id char(36) not null,
    data_referencia date not null,
    saldo decimal(18,2) not null,
    data_ultima_atualizacao timestamp not null,
    constraint primary key pk_saldo_consolidado_diario
        (comerciante_id, data_referencia)
);

CREATE TABLE evento_lancamento
(
    id char(36) not null,
    comerciante_id char(36) not null,
    valor decimal(18,2) not null,
    is_credito bool not null,
    data_criacao timestamp not null,
    constraint pk_evento_lancamento
        primary key (id)
);

-- jobs

USE saldo_consolidado;

SET GLOBAL event_scheduler = ON;