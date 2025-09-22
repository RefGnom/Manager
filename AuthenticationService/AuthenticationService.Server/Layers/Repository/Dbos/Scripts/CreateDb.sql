CREATE TABLE IF NOT EXISTS "authorizationModels"
(
    id                   uuid                  NOT NULL,
    owner                text                  NOT NULL,
    "availableServices"  text[] NOT NULL,
    "availableResources" text[] NOT NULL,
    "createdUtcTicks"    bigint                NOT NULL,
    "expirationUtcTicks" bigint,
    "Discriminator"      character varying(55) NOT NULL,
    "apiKeyHash"         text,
    CONSTRAINT "PK_authorizationModels" PRIMARY KEY (id)
);

alter table "authorizationModels" add column if not exists "isRevoked" boolean not null default false;