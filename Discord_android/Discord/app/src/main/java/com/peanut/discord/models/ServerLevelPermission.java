package com.peanut.discord.models;
public class ServerLevelPermission {
    private int roleId;
    private int permissionId;
    private boolean isActive;
    private Role role;
    private ServerPermission serverPermission;
    public int getRoleId() {
        return roleId;
    }
    public void setRoleId(int roleId) {
        this.roleId = roleId;
    }
    public int getPermissionId() {
        return permissionId;
    }
    public void setPermissionId(int permissionId) {
        this.permissionId = permissionId;
    }
    public boolean isActive() {
        return isActive;
    }
    public void setActive(boolean active) {
        isActive = active;
    }
    public Role getRole() {
        return role;
    }
    public void setRole(Role role) {
        this.role = role;
    }
    public ServerPermission getServerPermission() {
        return serverPermission;
    }
    public void setServerPermission(ServerPermission serverPermission) {
        this.serverPermission = serverPermission;
    }
}
