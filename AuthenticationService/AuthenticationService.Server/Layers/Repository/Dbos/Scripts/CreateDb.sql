CREATE TABLE "authorizationModels"
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
