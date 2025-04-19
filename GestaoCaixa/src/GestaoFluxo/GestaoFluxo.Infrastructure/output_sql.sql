CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `lancamentos` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `comerciante_id` char(36) COLLATE ascii_general_ci NOT NULL,
    `is_credito` tinyint(1) NOT NULL,
    `valor` decimal(18,2) NOT NULL,
    `data_criacao` datetime(6) NOT NULL,
    CONSTRAINT `PK_lancamentos` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_lancamentos_comerciante_id_is_credito_data_criacao` ON `lancamentos` (`comerciante_id`, `is_credito`, `data_criacao`);

CREATE INDEX `IX_lancamentos_data_criacao` ON `lancamentos` (`data_criacao`);

CREATE INDEX `IX_lancamentos_is_credito_data_criacao` ON `lancamentos` (`is_credito`, `data_criacao`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250413185701_Init', '8.0.15');

COMMIT;

START TRANSACTION;

ALTER TABLE `lancamentos` MODIFY COLUMN `data_criacao` datetime(6) NOT NULL DEFAULT (UTC_TIMESTAMP());

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250413190127_AddDtCriacaoPadraoUtc', '8.0.15');

COMMIT;

START TRANSACTION;

ALTER TABLE `lancamentos` MODIFY COLUMN `data_criacao` datetime(6) NOT NULL DEFAULT (UTC_TIMESTAMP(3));

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250413190901_AddDtCriacaoPadraoUtcPrecisao3', '8.0.15');

COMMIT;

START TRANSACTION;

CREATE TABLE `outbox_messages` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `tipo_evento` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
    `payload` varchar(5000) CHARACTER SET utf8mb4 NOT NULL,
    `destino` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `data_criacao` datetime(6) NOT NULL DEFAULT (UTC_TIMESTAMP(3)),
    `data_processamento` datetime(6) NULL,
    `is_processado` tinyint(1) NOT NULL,
    CONSTRAINT `PK_outbox_messages` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_outbox_messages_is_processado_data_criacao` ON `outbox_messages` (`is_processado`, `data_criacao`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250413232802_AddOutboxMessage', '8.0.15');

COMMIT;

