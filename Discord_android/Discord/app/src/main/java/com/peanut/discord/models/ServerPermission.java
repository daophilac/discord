package com.peanut.discord.models;
import java.util.List;
public class ServerPermission {
    private int permissionId;
    private String permissionName;
    private String description;
    private List<ServerLevelPermission> serverLevelPermissions;
    public int getPermissionId() {
        return permissionId;
    }
    public void setPermissionId(int permissionId) {
        this.permissionId = permissionId;
    }
    public String getPermissionName() {
        return permissionName;
    }
    public void setPermissionName(String permissionName) {
        this.permissionName = permissionName;
    }
    public String getDescription() {
        return description;
    }
    public void setDescription(String description) {
        this.description = description;
    }
    public List<ServerLevelPermission> getServerLevelPermissions() {
        return serverLevelPermissions;
    }
    public void setServerLevelPermissions(List<ServerLevelPermission> serverLevelPermissions) {
        this.serverLevelPermissions = serverLevelPermissions;
    }
}
